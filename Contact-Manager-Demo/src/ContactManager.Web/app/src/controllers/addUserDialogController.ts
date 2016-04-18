/// <reference path="../_all.ts" />

module ContactManagerApp {

    export class AddUserDialogController {
        static $inject = ['$mdDialog', '$mdToast', 'userService'];

        constructor(
            private $mdDialog: angular.material.IDialogService,
            private $mdToast: angular.material.IToastService,
            private userService: IUserService) { }

        userContact: UserContact;
        hasError: boolean = false;
        errorMessage: string;
        modelErrors: string;

        avatars = [
            'svg-1', 'svg-2', 'svg-3', 'svg-4', 'svg-5'
        ];

        cancel(): void {
            this.$mdDialog.cancel();
        }

        save(): void {
            this.userService.addContact(this.userContact)
                .then((result: any) => {
                    var data = result.data;
                    console.log(data);
                    this.openToast("User Contact added");
                    this.$mdDialog.hide(data);

                }).catch((errResult: any) => {
                    //on error
                    var data = errResult.data;
                    this.hasError = true;
                    this.errorMessage = data.message;
                    this.openToast(this.errorMessage);
                    console.log('error', data.errorMessage);

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