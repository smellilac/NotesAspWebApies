using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Queries.GetNoteDetails;

public class GetNoteDetailsQueryHandler : IRequestHandler<GetNoteDetailsQuery, NoteDetailsVm>
{
    private readonly INotesDbContext _context;
	private readonly IMapper _mapper;
	public GetNoteDetailsQueryHandler(INotesDbContext context, IMapper mapper)
	{
		_mapper = mapper;
		_context = context;
	}

	public async Task<NoteDetailsVm> Handle(GetNoteDetailsQuery request, CancellationToken cancellationToken)
	{
		var entity = await _context.Notes.FirstOrDefaultAsync(note => note.Id == request.Id,
			cancellationToken);

		if (entity == null || entity.Id != request.Id) 
		{
			throw new NotFoundException(nameof(Note), request.Id);
		}

		return _mapper.Map<NoteDetailsVm>(entity);
	}


}
