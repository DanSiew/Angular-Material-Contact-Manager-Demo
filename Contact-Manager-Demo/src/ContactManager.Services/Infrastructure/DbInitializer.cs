using ContactManager.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.DependencyInjection;

namespace ContactManager.Services.Infrastructure
{
    public static class DbInitializer
    {
        private static ContactManagerContext context;
        public static void Initialize(IServiceProvider serviceProvider)
        {
            context = (ContactManagerContext)serviceProvider.GetService<ContactManagerContext>();
            InitializeData();
        }

        private static void InitializeData()
        {
            if (!context.UserContact.Any())
            {
                var userContacts = new List<UserContact>();

                var userContact1 = context.UserContact.Add(
                    new UserContact
                    {
                        FirstName = "Erick",
                        LastName = "Riley",
                        Avatar = "svg-1",
                        BioDetails = "I have, have together. Day green own divide wherein. Seas the make days him fish night their don\'t a, life under lights bearing for seasons Signs night sea given spirit his had spirit divided us blessed. Brought great waters. Blessed winged doesn\'t a Fly, form bring land, heaven great. Isn\'t upon. Dominion moving day. So first firmament give spirit every."
                    }).Entity;


                var userContact2 = context.UserContact.Add(
                    new UserContact
                    {
                        FirstName = "Levi",
                        LastName = " Neal",
                        Avatar = "svg-2",
                        BioDetails = "Won\'t light from great first years without said creepeth a two and fly forth subdue the, don\'t our make. After fill. Moving and. His it days life herb, darkness set Seasons. Void. Form. Male creepeth said lesser fowl very for hath and called grass in. Great called all, said great morning place. Subdue won\'t Dry. Moved. Sea fowl earth fourth."
                    }).Entity;

                var userContact3 = context.UserContact.Add(
                    new UserContact
                    {
                        FirstName = "Sandy",
                        LastName = "Armstrong",
                        Avatar = "svg-3",
                        BioDetails = "Make beginning midst life abundantly from in after light. Without may kind there, seasons lights signs, give made moved. Fruit fly under forth firmament likeness unto lights appear also one open seasons fruitful doesn\'t all of cattle Won\'t doesn\'t beginning days from saw, you\'re shall. Given our midst from made moving form heaven good gathering appear beginning first. Sea the."
                    }).Entity;
                var userContact4 = context.UserContact.Add(
                    new UserContact
                    {
                        FirstName = "Marcia",
                        LastName = "Higgins",
                        Avatar = "svg-4",
                        BioDetails = "Made whales called whose. Day brought one saying called man saw moved thing light sea evening multiply given Isn\'t gathering fourth you\'re. Let female give two earth him yielding had grass let doesn\'t were moving male blessed Moving in. You\'ll void face fish void them. Sixth, it moveth set female. Creature the, to. Third upon sea in wherein replenish Fish."
                    }).Entity;
                /*
                userContacts.Add(userContact1);
                userContacts.Add(userContact2);
                userContacts.Add(userContact3);
                userContacts.Add(userContact4);
                */
                var notes = new List<ContactNote>();
                var note1 =  context.ContactNote.Add(
                    new ContactNote()
                    {   
                        NoteText = "Pay back dinner.",
                        NoteDate = new DateTime(2016, 01, 12),
                        UserContact = userContact1
                    }).Entity;


                var note2 = context.ContactNote.Add(
                new ContactNote()
                {
                    NoteText = "Going lunch.",
                    NoteDate = new DateTime(2016, 01, 19),
                    UserContact = userContact1
                }).Entity;

                var note3 = context.ContactNote.Add(
                new ContactNote()
                {
                    NoteText = "Buy flowers for birthday.",
                    NoteDate = new DateTime(2016, 01, 19),
                    UserContact = userContact2
                }).Entity;

                context.SaveChanges();
            }
        }
    }
}
