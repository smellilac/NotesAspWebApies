using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Commands.DeleteNote;

public class DeleteNoteCommandValidator : AbstractValidator<DeleteNoteCommand>
{
    public DeleteNoteCommandValidator()
    {
        RuleFor(updateNoteCommand =>
           updateNoteCommand.UserId).NotEqual(Guid.Empty);
        RuleFor(updateNoteCommand =>
            updateNoteCommand.Id).NotEqual(Guid.Empty);

    }
}
