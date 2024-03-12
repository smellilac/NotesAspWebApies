using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Queries.GetNoteList;

public class GetNoteListQueryHandler : IRequestHandler<GetNoteListQuery, NoteListVm>
{
    private readonly INotesDbContext _context;
    private readonly IMapper _mapper;

    public GetNoteListQueryHandler(INotesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<NoteListVm> Handle(GetNoteListQuery request, CancellationToken cancellationToken)
    {
        var notesQuery = await _context.Notes.Where(note => note.UserId == request.UserId)
            .ProjectTo<NoteLookUpDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        if (notesQuery == null)
        {
            throw new NotFoundException(nameof(Note), request.Id);
        }
        else
        {
            return new NoteListVm { Notes = notesQuery };
        }
    }
}
