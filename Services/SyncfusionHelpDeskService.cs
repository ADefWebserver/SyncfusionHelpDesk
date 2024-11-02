#nullable disable
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SyncfusionHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncfusionHelpDesk.Data
{
    public class SyncfusionHelpDeskService
    {
        public SyncfusionHelpDeskService() { }

        public IQueryable<HelpDeskTicket>
            GetHelpDeskTickets(IDbContextFactory<SyncfusionHelpDeskContext> dbContextFactory, bool IsAdmin, string paramEmail)
        {
            // Return all HelpDesk Tickets as IQueryable
            // SfGrid will use this to only pull records 
            // for the page that it is currently displaying
            // Note: AsNoTracking() is used because it is 
            // quicker to execute and we do not need
            // Entity Framework change tracking at this point

            var _context = dbContextFactory.CreateDbContext();

            if (IsAdmin)
            {
                // Admin User
                return _context.HelpDeskTickets.AsNoTracking();
            }
            else
            {
                // Regular User
                return _context.HelpDeskTickets
                    .Where(x => x.TicketRequesterEmail == paramEmail)
                    .AsNoTracking();
            }
        }

        public async Task<HelpDeskTicket>
            GetHelpDeskTicketAsync(IDbContextFactory<SyncfusionHelpDeskContext> dbContextFactory, string HelpDeskTicketGuid)
        {
            // Get the existing record
            var _context = dbContextFactory.CreateDbContext();

            var ExistingTicket = await _context.HelpDeskTickets
                .Include(x => x.HelpDeskTicketDetails)
                .Where(x => x.TicketGuid == HelpDeskTicketGuid)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return ExistingTicket;
        }

        public Task<HelpDeskTicket>
            CreateTicketAsync(IDbContextFactory<SyncfusionHelpDeskContext> dbContextFactory, HelpDeskTicket newHelpDeskTickets)
        {
            // Add a new Help Desk Ticket
            var _context = dbContextFactory.CreateDbContext();

            _context.HelpDeskTickets.Add(newHelpDeskTickets);
            _context.SaveChanges();

            return Task.FromResult(newHelpDeskTickets);
        }

        public Task<bool>
            UpdateTicketAsync(IDbContextFactory<SyncfusionHelpDeskContext> dbContextFactory,
            HelpDeskTicket UpdatedHelpDeskTickets)
        {
            // Get the existing record
            var _context = dbContextFactory.CreateDbContext();

            var ExistingTicket =
                _context.HelpDeskTickets
                .Where(x => x.Id == UpdatedHelpDeskTickets.Id)
                .FirstOrDefault();

            if (ExistingTicket != null)
            {
                ExistingTicket.TicketDate =
                    UpdatedHelpDeskTickets.TicketDate;

                ExistingTicket.TicketDescription =
                    UpdatedHelpDeskTickets.TicketDescription;

                ExistingTicket.TicketGuid =
                    UpdatedHelpDeskTickets.TicketGuid;

                ExistingTicket.TicketRequesterEmail =
                    UpdatedHelpDeskTickets.TicketRequesterEmail;

                ExistingTicket.TicketStatus =
                    UpdatedHelpDeskTickets.TicketStatus;

                // Insert any new TicketDetails
                if (UpdatedHelpDeskTickets.HelpDeskTicketDetails != null)
                {
                    foreach (var item in
                        UpdatedHelpDeskTickets.HelpDeskTicketDetails)
                    {
                        if (item.Id == 0)
                        {
                            // Create New HelpDeskTicketDetails record
                            HelpDeskTicketDetail newHelpDeskTicketDetails =
                                new HelpDeskTicketDetail();
                            newHelpDeskTicketDetails.HelpDeskTicketId =
                                UpdatedHelpDeskTickets.Id;
                            newHelpDeskTicketDetails.TicketDetailDate =
                                DateTime.Now;
                            newHelpDeskTicketDetails.TicketDescription =
                                item.TicketDescription;

                            _context.HelpDeskTicketDetails
                                .Add(newHelpDeskTicketDetails);
                        }
                    }
                }

                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<bool>
            DeleteHelpDeskTicketsAsync(IDbContextFactory<SyncfusionHelpDeskContext> dbContextFactory,
            HelpDeskTicket DeleteHelpDeskTickets)
        {
            // Get the existing record
            var _context = dbContextFactory.CreateDbContext();

            var ExistingTicket =
                _context.HelpDeskTickets
                .Include(x => x.HelpDeskTicketDetails)
                .Where(x => x.Id == DeleteHelpDeskTickets.Id)
                .FirstOrDefault();

            if (ExistingTicket != null)
            {
                // Delete the Help Desk Ticket
                _context.HelpDeskTickets.Remove(ExistingTicket);
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
