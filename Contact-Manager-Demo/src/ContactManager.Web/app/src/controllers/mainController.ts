/// <reference path="../_all.ts" />

module ContactManagerApp {
    export class MainController {
        static $inject = [
            'userService',
            '$mdSidenav',
            '$mdToast',
            '$mdDialog',
            '$mdMedia',
            '$mdBottomSheet'];

        constructor(
            private userService: IUserService,
            private $mdSidenav: angular.material.ISidenavService,
            private $mdToast: angular.material.IToastService,
            private $mdDialog: angular.material.IDialogService,
            private $mdMedia: angular.material.IMedia,
            private $mdBottomSheet: angular.material.IBottomSheetService) {
            var self = this;
            this.userService.queryAll().then((result: any) => {
                var data = result.data;
                self.users = data;
                self.selected = self.users[0];
                self.userService.selectedUser = self.selected;
                console.log(self.users);

            });
        }

        tabIndex: number = 0;
        searchText: string = '';
        users: UserContact[] = [];
        selected: UserContact = null;
        newNote: ContactNote = null;
        hasError: boolean = false;
        errorMessage: string;
        modelErrors: string;

        toggleSideNav(): void {
            this.$mdSidenav('left').toggle();
        }

        selectUser(user: UserContact): void {
            this.selected = user;
            this.userService.selectedUser = user;
            this.newNote = new ContactNote(this.selected, 0, '', null);
            console.log('user', this.selected);
            var sidenav = this.$mdSidenav('left');
            if (sidenav.isOpen()) {
                sidenav.close();
            }

            this.tabIndex = 0;
        }

        showContactOptions($event) {
            this.$mdBottomSheet.show({
                parent: angular.element(document.getElementById('wrapper')),
                templateUrl: './dist/view/contactSheet.html',
                controller: ContactPanelController,
                controllerAs: "cp",
                bindToController: true,
                targetEvent: $event
            }).then((clickedItem) => {
                clickedItem && console.log(clickedItem.name + ' clicked!');
            })
        }

        removeUserContact($event) {
            var self = this;
            var confirm = this.$mdDialog.confirm()
                .title('Are you sure you want to delete the selected contact ' + this.selected.name + ' ?')
                .textContent('The contact will be deleted, you can\'t undo this action.')
                .targetEvent($event)
                .ok('Yes')
                .cancel('No');

            this.$mdDialog.show(confirm).then(() => {
                var foundIndex = this.users.indexOf(this.selected);
                this.userService.removeContact(this.selected.id)
                    .then((result: any) => {
                        var data = result.data;
                        console.log('remove note', data);
                        this.users.splice(foundIndex, 1);
                        if (self.users.length > 0) {
                            self.selected = self.users[0];
                        } else {
                            self.selected = null;
                        }
                        self.openToast('Contact deleted');

                    }).catch((errResult: any) => {
                        //on error
                        var data = errResult.data;
                        this.hasError = true;
                        this.errorMessage = data.message;
                        this.openToast(this.errorMessage);
                        console.log('error', this.errorMessage);

                        if (data.modelState !== undefined) {
                            this.modelErrors = data.modelState.error;
                        }
                    });
                
            })
        }

        addUserContact($event) {
            var self = this;
            var useFullScreen = (this.$mdMedia('sm') || this.$mdMedia('xs'));

            this.$mdDialog.show({
                templateUrl: './dist/view/newUserDialog.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                controller: AddUserDialogController,
                controllerAs: 'ctrlUserDialog',
                clickOutsideToClose: true,
                fullscreen: useFullScreen
            }).then((userContact: UserContact) => {
                var newUser: UserContact = userContact;
                console.log('newUser', newUser);
                self.users.push(newUser);
                self.selectUser(newUser);
            }, () => {
                console.log('You cancelled the dialog.');
            });
        }

        clearNotes($event) {
            var confirm = this.$mdDialog.confirm()
                .title('Are you sure you want to delete all notes?')
                .textContent('All notes will be deleted, you can\'t undo this action.')
                .targetEvent($event)
                .ok('Yes')
                .cancel('No');

            var self = this;
            this.$mdDialog.show(confirm).then(() => {
                self.selected.contactNotes = [];
                self.openToast('Cleared notes');
            })
        }

        formScope: any;

        setFormScope(scope) {
            this.formScope = scope;
        }

        addNote() {
            var note = new ContactNote(this.selected, 0, this.newNote.noteText, this.newNote.noteDate);
            this.userService.addNote(note)
                .then((result: any) => {
                var data = result.data;
                console.log(data);
                this.selected.contactNotes.push(data);
                this.openToast("Note added");

                // reset the form
                this.formScope.noteForm.$setUntouched();
                this.formScope.noteForm.$setPristine();

                this.newNote = new ContactNote(this.selected, 0, '', null);

                }).catch((errResult: any) => {
                    //on error
                    var data = errResult.data;
                    this.hasError = true;
                    this.errorMessage = data.message;
                    this.openToast(this.errorMessage);
                    console.log('error', this.errorMessage);

                    if (data.modelState !== undefined) {
                        this.modelErrors = data.modelState.error;
                    }
                });

            
        }

        removeNote(note: ContactNote): void {
            var foundIndex = this.selected.contactNotes.indexOf(note);
            this.userService.removeNote(note.id)
                .then((result: any) => {
                    var data = result.data;
                    console.log('remove note', data);
                    this.selected.contactNotes.splice(foundIndex, 1);
                    this.openToast("Note was removed");

                }).catch((errResult: any) => {
                    //on error
                    var data = errResult.data;
                    this.hasError = true;
                    this.errorMessage = data.message;
                    this.openToast(this.errorMessage);
                    console.log('error', this.errorMessage);

                    if (data.modelState !== undefined) {
                        this.modelErrors = data.modelState.error;
                    }
                });


        }

        openToast(message: string): void {
            this.$mdToast.show(
                this.$mdToast.simple()
                    .textContent(message)
                    .position('top right')
                    .hideDelay(3000)
            );
        }
    }
}