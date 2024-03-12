using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Queries.GetNoteDetails;

public class GetNoteDetailsQueryValidator : AbstractValidator<GetNoteDetailsQuery>
{
    public GetNoteDetailsQueryValidator()
    {
        RuleFor(note =>
           note.UserId).NotEqual(Guid.Empty);
        RuleFor(note =>
            note.Id).NotEqual(Guid.Empty);

    }
}
