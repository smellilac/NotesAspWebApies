﻿using FluentValidation;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Queries.GetNoteList;

public class GetNoteListQueryValidator : AbstractValidator<GetNoteListQuery>
{
    public GetNoteListQueryValidator()
    {
        RuleFor(note =>
          note.UserId).NotEqual(Guid.Empty);
        RuleFor(note =>
            note.Id).NotEqual(Guid.Empty);
    }
}
