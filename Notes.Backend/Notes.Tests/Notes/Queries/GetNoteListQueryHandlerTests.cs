using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Queries.GetNoteDetails;
using Notes.Application.Queries.GetNoteList;
using Notes.Tests.Common;
using Notes.WebApi.PersistanceLevel;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Queries;
[Collection("QueryCollection")]
public class GetNoteListQueryHandlerTests : TestCommandBase
{
    private readonly NotesDbContext _context;
    private readonly IMapper _mapper;

    public GetNoteListQueryHandlerTests(QueryTestFixture fixture)
    {
        _context = fixture.Context;
        _mapper = fixture.Mapper;
    }
    [Fact]
    public async Task GetNoteListQueryTests_Succes()
    {
        // Arrange
        var handler = new GetNoteListQueryHandler(_context, _mapper);
        //Act
        var result = await handler.Handle(new GetNoteListQuery
        {
            UserId = NotesContextFactory.UserBId
        }, CancellationToken.None);
        //Assert
        result.ShouldBeOfType<NoteListVm>();
        result.Notes.Count.ShouldBe(2);

    }
}
