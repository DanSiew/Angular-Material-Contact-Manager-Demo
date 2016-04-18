/// <reference path="../_all.ts" />
var ContactManagerApp;
(function (ContactManagerApp) {
    var UserService = (function () {
        function UserService($q, $http) {
            this.$q = $q;
            this.selectedUser = null;
            this.$http = $http;
            this.url = 'http://localhost:64000/';
        }
        UserService.prototype.queryAll = function () {
            var config = {
                headers: ''
            };
            return this.$http.get(this.url + 'api/usercontact', config)
                .success(function (response) {
                return response.data;
            })
                .error(function (errResponse) {
                return errResponse;
            });
        };
        UserService.prototype.addContact = function (input) {
            var _this = this;
            var data = JSON.stringify(input);
            var config = {
                headers: ''
            };
            var result = this.$http.post(this.url + 'api/usercontact', data, config)
                .success(function (response) {
                return _this.updateHandler(response);
            })
                .error(function (errResponse) {
                return _this.updateHandler(errResponse);
            });
            return result;
        };
        UserService.prototype.addNote = function (input) {
            var _this = this;
            var data = JSON.stringify(input);
            var config = {
                headers: ''
            };
            var result = this.$http.post(this.url + 'api/contactnote', data, config)
                .success(function (response) {
                return _this.updateHandler(response);
            })
                .error(function (errResponse) {
                return _this.updateHandler(errResponse);
            });
            return result;
        };
        UserService.prototype.removeNote = function (input) {
            var _this = this;
            var config = {
                headers: ''
            };
            var result = this.$http.delete(this.url + 'api/contactnote/' + input, config)
                .success(function (response) {
                return _this.updateHandler(response);
            })
                .error(function (errResponse) {
                return _this.updateHandler(errResponse);
            });
            return result;
        };
        UserService.prototype.updateHandler = function (response) {
            return response;
        };
        UserService.$inject = ['$q', '$http'];
        return UserService;
    }());
    ContactManagerApp.UserService = UserService;
})(ContactManagerApp || (ContactManagerApp = {}));
//# sourceMappingURL=userService.js.map
/// <reference path="../_all.ts" />
var ContactManagerApp;
(function (ContactManagerApp) {
    var MainController = (function () {
        function MainController(userService, $mdSidenav, $mdToast, $mdDialog, $mdMedia, $mdBottomSheet) {
            this.userService = userService;
            this.$mdSidenav = $mdSidenav;
            this.$mdToast = $mdToast;
            this.$mdDialog = $mdDialog;
            this.$mdMedia = $mdMedia;
            this.$mdBottomSheet = $mdBottomSheet;
            this.tabIndex = 0;
            this.searchText = '';
            this.users = [];
            this.selected = null;
            this.newNote = null;
            this.hasError = false;
            var self = this;
            this.userService.queryAll().then(function (result) {
                var data = result.data;
                self.users = data;
                self.selected = self.users[0];
                self.userService.selectedUser = self.selected;
                console.log(self.users);
            });
        }
        MainController.prototype.toggleSideNav = function () {
            this.$mdSidenav('left').toggle();
        };
        MainController.prototype.selectUser = function (user) {
            this.selected = user;
            this.userService.selectedUser = user;
            this.newNote = new ContactManagerApp.ContactNote(this.selected, 0, '', null);
            console.log('user', this.selected);
            var sidenav = this.$mdSidenav('left');
            if (sidenav.isOpen()) {
                sidenav.close();
            }
            this.tabIndex = 0;
        };
        MainController.prototype.showContactOptions = function ($event) {
            this.$mdBottomSheet.show({
                parent: angular.element(document.getElementById('wrapper')),
                templateUrl: './dist/view/contactSheet.html',
                controller: ContactManagerApp.ContactPanelController,
                controllerAs: "cp",
                bindToController: true,
                targetEvent: $event
            }).then(function (clickedItem) {
                clickedItem && console.log(clickedItem.name + ' clicked!');
            });
        };
        MainController.prototype.addUserContact = function ($event) {
            var self = this;
            var useFullScreen = (this.$mdMedia('sm') || this.$mdMedia('xs'));
            this.$mdDialog.show({
                templateUrl: './dist/view/newUserDialog.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                controller: ContactManagerApp.AddUserDialogController,
                controllerAs: 'ctrlUserDialog',
                clickOutsideToClose: true,
                fullscreen: useFullScreen
            }).then(function (userContact) {
                var newUser = userContact;
                console.log('newUser', newUser);
                self.users.push(newUser);
                self.selectUser(newUser);
            }, function () {
                console.log('You cancelled the dialog.');
            });
        };
        MainController.prototype.clearNotes = function ($event) {
            var confirm = this.$mdDialog.confirm()
                .title('Are you sure you want to delete all notes?')
                .textContent('All notes will be deleted, you can\'t undo this action.')
                .targetEvent($event)
                .ok('Yes')
                .cancel('No');
            var self = this;
            this.$mdDialog.show(confirm).then(function () {
                self.selected.contactNotes = [];
                self.openToast('Cleared notes');
            });
        };
        MainController.prototype.setFormScope = function (scope) {
            this.formScope = scope;
        };
        MainController.prototype.addNote = function () {
            var _this = this;
            var note = new ContactManagerApp.ContactNote(this.selected, 0, this.newNote.noteText, this.newNote.noteDate);
            this.userService.addNote(note)
                .then(function (result) {
                var data = result.data;
                console.log(data);
                _this.selected.contactNotes.push(data);
                _this.openToast("Note added");
                // reset the form
                _this.formScope.noteForm.$setUntouched();
                _this.formScope.noteForm.$setPristine();
                _this.newNote = new ContactManagerApp.ContactNote(_this.selected, 0, '', null);
            }).catch(function (errResult) {
                //on error
                var data = errResult.data;
                _this.hasError = true;
                _this.errorMessage = data.message;
                _this.openToast(_this.errorMessage);
                console.log('error', _this.errorMessage);
                if (data.modelState !== undefined) {
                    _this.modelErrors = data.modelState.error;
                }
            });
        };
        MainController.prototype.removeNote = function (note) {
            var _this = this;
            var foundIndex = this.selected.contactNotes.indexOf(note);
            this.userService.removeNote(note.id)
                .then(function (result) {
                var data = result.data;
                console.log('remove note', data);
                _this.selected.contactNotes.splice(foundIndex, 1);
                _this.openToast("Note was removed");
            }).catch(function (errResult) {
                //on error
                var data = errResult.data;
                _this.hasError = true;
                _this.errorMessage = data.message;
                _this.openToast(_this.errorMessage);
                console.log('error', _this.errorMessage);
                if (data.modelState !== undefined) {
                    _this.modelErrors = data.modelState.error;
                }
            });
        };
        MainController.prototype.openToast = function (message) {
            this.$mdToast.show(this.$mdToast.simple()
                .textContent(message)
                .position('top right')
                .hideDelay(3000));
        };
        MainController.$inject = [
            'userService',
            '$mdSidenav',
            '$mdToast',
            '$mdDialog',
            '$mdMedia',
            '$mdBottomSheet'];
        return MainController;
    }());
    ContactManagerApp.MainController = MainController;
})(ContactManagerApp || (ContactManagerApp = {}));
//# sourceMappingURL=mainController.js.map
/// <reference path="../_all.ts" />
var ContactManagerApp;
(function (ContactManagerApp) {
    var ContactPanelController = (function () {
        function ContactPanelController(userService, $mdBottomSheet) {
            this.userService = userService;
            this.$mdBottomSheet = $mdBottomSheet;
            this.actions = [
                { name: 'Phone', icon: 'phone' },
                { name: 'Twitter', icon: 'twitter' },
                { name: 'Google+', icon: 'google_plus' },
                { name: 'Hangout', icon: 'hangouts' }
            ];
            this.user = userService.selectedUser;
        }
        ContactPanelController.prototype.submitContact = function (action) {
            this.$mdBottomSheet.hide(action);
        };
        ContactPanelController.$inject = ['userService', '$mdBottomSheet'];
        return ContactPanelController;
    }());
    ContactManagerApp.ContactPanelController = ContactPanelController;
})(ContactManagerApp || (ContactManagerApp = {}));
//# sourceMappingURL=contactPanelController.js.map
/// <reference path="../_all.ts" />
var ContactManagerApp;
(function (ContactManagerApp) {
    var AddUserDialogController = (function () {
        function AddUserDialogController($mdDialog, $mdToast, userService) {
            this.$mdDialog = $mdDialog;
            this.$mdToast = $mdToast;
            this.userService = userService;
            this.hasError = false;
            this.avatars = [
                'svg-1', 'svg-2', 'svg-3', 'svg-4', 'svg-5'
            ];
        }
        AddUserDialogController.prototype.cancel = function () {
            this.$mdDialog.cancel();
        };
        AddUserDialogController.prototype.save = function () {
            var _this = this;
            this.userService.addContact(this.userContact)
                .then(function (result) {
                var data = result.data;
                console.log(data);
                _this.openToast("User Contact added");
                _this.$mdDialog.hide(data);
            }).catch(function (errResult) {
                //on error
                var data = errResult.data;
                _this.hasError = true;
                _this.errorMessage = data.message;
                _this.openToast(_this.errorMessage);
                console.log('error', data.errorMessage);
                if (data.modelState !== undefined) {
                    _this.modelErrors = data.modelState.error;
                }
            });
        };
        AddUserDialogController.prototype.openToast = function (message) {
            this.$mdToast.show(this.$mdToast.simple()
                .textContent(message)
                .position('top right')
                .hideDelay(3000));
        };
        AddUserDialogController.$inject = ['$mdDialog', '$mdToast', 'userService'];
        return AddUserDialogController;
    }());
    ContactManagerApp.AddUserDialogController = AddUserDialogController;
})(ContactManagerApp || (ContactManagerApp = {}));
//# sourceMappingURL=addUserDialogController.js.map
/// <reference path="_all.ts" />
var ContactManagerApp;
(function (ContactManagerApp) {
    var CreateUser = (function () {
        function CreateUser(firstName, lastName, avatar, bioDetails) {
            this.firstName = firstName;
            this.lastName = lastName;
            this.avatar = avatar;
            this.bioDetails = bioDetails;
        }
        return CreateUser;
    }());
    ContactManagerApp.CreateUser = CreateUser;
    var UserContact = (function () {
        function UserContact(id, firstName, lastName, name, avatar, bioDetails, contactNotes) {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.name = name;
            this.avatar = avatar;
            this.bioDetails = bioDetails;
            this.contactNotes = contactNotes;
        }
        return UserContact;
    }());
    ContactManagerApp.UserContact = UserContact;
    var ContactNote = (function () {
        function ContactNote(userContact, id, noteText, noteDate) {
            this.userContact = userContact;
            this.id = id;
            this.noteText = noteText;
            this.noteDate = noteDate;
        }
        return ContactNote;
    }());
    ContactManagerApp.ContactNote = ContactNote;
})(ContactManagerApp || (ContactManagerApp = {}));
//# sourceMappingURL=models.js.map
/// <reference path='_all.ts' />
var ContactManagerApp;
(function (ContactManagerApp) {
    angular.module('contactManagerApp', ['ngMaterial', 'ngMdIcons'])
        .service('userService', ContactManagerApp.UserService)
        .controller('mainController', ContactManagerApp.MainController)
        .config(function ($mdIconProvider, $mdThemingProvider) {
        $mdIconProvider
            .defaultIconSet('./svg/avatars.svg', 128)
            .icon('google_plus', './svg/google_plus.svg', 512)
            .icon('hangouts', './svg/hangouts.svg', 512)
            .icon('twitter', './svg/twitter.svg', 512)
            .icon('phone', './svg/phone.svg', 512)
            .icon('menu', './svg/menu.svg', 24);
        $mdThemingProvider.theme('default')
            .primaryPalette('blue')
            .accentPalette('red');
    });
})(ContactManagerApp || (ContactManagerApp = {}));
//# sourceMappingURL=boot.js.map