/// <reference path="../_all.ts" />

module ContactManagerApp {

    export interface IUserService {
        queryAll(): ng.IPromise<UserContact>;
        addContact(input: UserContact): any;
        addNote(input: ContactNote): any;
        removeNote(input: number): any;
        selectedUser: UserContact;
    }

    export class UserService implements IUserService {
        static $inject = ['$q', '$http'];
        $http: ng.IHttpService;
        url: string;

        constructor(private $q: ng.IQService, $http: ng.IHttpService) {
            this.$http = $http;
            this.url = 'http://localhost:64000/' ;
        }

        selectedUser: UserContact = null;

        queryAll(): ng.IPromise<UserContact> {
            var config: any = {
                headers: ''
            };

            return this.$http.get(this.url + 'api/usercontact', config)
                .success((response: ng.IHttpPromiseCallbackArg<UserContact>): UserContact => {
                    return <UserContact>response.data;
                })
                .error((errResponse: ng.IHttpPromiseCallback<Error>): any => {
                    return errResponse;
                });
        }

        addContact(input: UserContact): any {
            var data = JSON.stringify(input);
            var config: any = {
                headers: ''
            };
            var result: ng.IPromise<any> = this.$http.post(this.url + 'api/usercontact', data, config)
                .success((response: ng.IHttpPromiseCallbackArg<ContactNote>):
                    ng.IPromise<any> => this.updateHandler(response))
                .error((errResponse: ng.IHttpPromiseCallback<Error>):
                    ng.IPromise<any> => this.updateHandler(errResponse))
            return result;

        }


        addNote(input: ContactNote): any {
            var data = JSON.stringify(input);
            var config: any = {
                headers: ''
            };
            var result: ng.IPromise<any> = this.$http.post(this.url + 'api/contactnote', data, config)
                .success((response: ng.IHttpPromiseCallbackArg<ContactNote>):
                    ng.IPromise<any> => this.updateHandler(response))
                .error((errResponse: ng.IHttpPromiseCallback<Error>):
                    ng.IPromise<any> => this.updateHandler(errResponse))
            return result;

        }

        removeNote(input: number): any {
            var config: any = {
                headers: ''
            };
            var result: ng.IPromise<any> = this.$http.delete(this.url + 'api/contactnote/' + input, config)
                .success((response: ng.IHttpPromiseCallbackArg<ContactNote>):
                    ng.IPromise<any> => this.updateHandler(response))
                .error((errResponse: ng.IHttpPromiseCallback<Error>):
                    ng.IPromise<any> => this.updateHandler(errResponse))
            return result;

        }

        updateHandler(response: any): any {
            return response;
        }
    }
}