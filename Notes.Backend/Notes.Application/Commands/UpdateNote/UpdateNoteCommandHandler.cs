using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Commands.UpdateNote;

public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand>
{
    private readonly INotesDbContext _context;

    public UpdateNoteCommandHandler(INotesDbContext dbContext)
    {
        _context = dbContext;
    }
    public async Task Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        var entity = 
            await _context.Notes.FirstOrDefaultAsync(note => note.Id == request.Id, cancellationToken);
        if (entity == null || entity.UserId != request.UserId)
            throw new NotFoundException(nameof(Note), request.Id);

        entity.Details = request.Details;
        entity.Title = request.Title;
        entity.EditDate = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);

    }
}
