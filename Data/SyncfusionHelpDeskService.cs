using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncfusionHelpDesk.Data
{
    public class SyncfusionHelpDeskService
    {
        private readonly SyncfusionHelpDeskContext _context;

        public SyncfusionHelpDeskService(
            SyncfusionHelpDeskContext context)
        {
            _context = context;
        }

        public IQueryable<HelpDeskTickets>
            GetHelpDeskTickets()
        {
            return _context.HelpDeskTickets
                .AsNoTracking();
        }

        public async Task<HelpDeskTickets>
            GetHelpDeskTicketAsync(string HelpDeskTicketGuid)
        {
            var ExistingTicket = await _context.HelpDeskTickets
                .Include(x => x.HelpDeskTicketDetails)
                .Where(x => x.TicketGuid == HelpDeskTicketGuid)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return ExistingTicket;
        }

        public Task<HelpDeskTickets>
            CreateTicketAsync(HelpDeskTickets newHelpDeskTickets)
        {
            try
            {
                _context.HelpDeskTickets.Add(newHelpDeskTickets);
                _context.SaveChanges();

                return Task.FromResult(newHelpDeskTickets);
            }
            catch (Exception ex)
            {
                DetachAllEntities();
                throw ex;
            }
        }

        public Task<HelpDeskTicketDetails>
            CreateTicketDetailAsync(
            HelpDeskTicketDetails newHelpDeskTicketDetails)
        {
            try
            {
                _context.HelpDeskTicketDetails
                    .Add(newHelpDeskTicketDetails);

                _context.SaveChanges();

                return Task.FromResult(newHelpDeskTicketDetails);
            }
            catch (Exception ex)
            {
                DetachAllEntities();
                throw ex;
            }
        }

        public Task<bool>
            UpdateTicketAsync(
            HelpDeskTickets UpdatedHelpDeskTickets)
        {
            try
            {
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
                            if(item.Id == 0)
                            {
                                // Create New HelpDeskTicketDetails record
                                HelpDeskTicketDetails newHelpDeskTicketDetails = new HelpDeskTicketDetails();
                                newHelpDeskTicketDetails.HelpDeskTicketId = UpdatedHelpDeskTickets.Id;
                                newHelpDeskTicketDetails.TicketDetailDate = DateTime.Now;
                                newHelpDeskTicketDetails.TicketDescription = item.TicketDescription;

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
            catch (Exception ex)
            {
                DetachAllEntities();
                throw ex;
            }
        }

        public Task<bool>
            DeleteHelpDeskTicketsAsync(
            HelpDeskTickets DeleteHelpDeskTickets)
        {
            var ExistingTicket =
                _context.HelpDeskTickets
                .Include(x => x.HelpDeskTicketDetails)
                .Where(x => x.Id == DeleteHelpDeskTickets.Id)
                .FirstOrDefault();

            if (ExistingTicket != null)
            {
                _context.HelpDeskTickets.Remove(ExistingTicket);
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        // Utility

        #region public void DetachAllEntities()
        public void DetachAllEntities()
        {
            // When we have an error we need 
            // to remove EF Core change tracking
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
        #endregion

    }
}
