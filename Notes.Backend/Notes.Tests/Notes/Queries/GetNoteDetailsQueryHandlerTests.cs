using AutoMapper;
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

public class GetNoteDetailsQueryHandlerTests : TestCommandBase
{
    private readonly NotesDbContext _context;
    private readonly IMapper _mapper;

    public GetNoteDetailsQueryHandlerTests(QueryTestFixture fixture)
    {
        _context = fixture.Context;
        _mapper = fixture.Mapper;
    }

    [Fact]
    public async Task GetNoteDetailsQueryHandler_Succes()
    {
        // Arrange
        var handler = new GetNoteDetailsQueryHandler(_context, _mapper);
        //Act
        var result = await handler.Handle(new GetNoteDetailsQuery
        {
            UserId = NotesContextFactory.UserBId,
            Id = Guid.Parse("909F7C29-891B-4BE1-8504-21F84F262084")
        }, CancellationToken.None);
        //Assert
        result.ShouldBeOfType<NoteDetailsVm>();
        result.Title.ShouldBe("Title2");
        result.CreationDate.ShouldBe(DateTime.Today);
    }

}
