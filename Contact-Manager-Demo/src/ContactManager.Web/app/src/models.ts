/// <reference path="_all.ts" />

module ContactManagerApp {

    export class CreateUser {
        constructor(
            public firstName: string,
            public lastName: string,
            public avatar: string,
            public bioDetails: string) {
        }
    }

    export class UserContact {
        constructor(
            public id: number,
            public firstName: string,
            public lastName: string,
            public name: string,
            public avatar: string,
            public bioDetails: string,
            public contactNotes: ContactNote[]) {
        }


    }

    export class ContactNote {
        constructor(
            public userContact: UserContact,
            public id: number,
            public noteText: string,
            public noteDate: Date) {
        }
    }

}