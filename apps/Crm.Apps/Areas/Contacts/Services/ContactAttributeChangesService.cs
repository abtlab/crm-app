﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Areas.Contacts.Models;
using Crm.Apps.Areas.Contacts.Parameters;
using Crm.Apps.Areas.Contacts.Storages;
using Crm.Apps.Utils;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Contacts.Services
{
    public class ContactAttributeChangesService : IContactAttributeChangesService
    {
        private readonly ContactsStorage _storage;

        public ContactAttributeChangesService(ContactsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<ContactAttributeChange>> GetPagedListAsync(
            ContactAttributeChangeGetPagedListParameter parameter, CancellationToken ct)
        {
            return _storage.ContactAttributeChanges
                .AsNoTracking()
                .Where(x =>
                    (parameter.ChangerUserId.IsEmpty() || x.ChangerUserId == parameter.ChangerUserId) &&
                    (parameter.AttributeId.IsEmpty() || x.AttributeId == parameter.AttributeId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .SortBy(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}