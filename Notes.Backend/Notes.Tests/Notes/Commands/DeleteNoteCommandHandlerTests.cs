using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Notes.Application.Commands.CreateNote;
using Notes.Application.Commands.DeleteNote;
using Notes.Application.Common.Exceptions;
using Notes.Application.Queries.GetNoteDetails;
using Notes.Tests.Common;
using Notes.WebApi.PersistanceLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Commands;

public class DeleteNoteCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task DeleteNoteCommandHandlerTests_Succes()
    {
        //Arrange 
        var handler = new DeleteNoteCommandHandler(context);
        //Act
        await handler.Handle(new DeleteNoteCommand
        {
            UserId = NotesContextFactory.UserAId,
            Id = NotesContextFactory.NoteIdForDelete
        },
        CancellationToken.None);
        // Assert

        //если UserId удаляющего заметку не соответствует
        //UserId указанному в заметку - тоже бросам исключение
        Assert.Null(context.Notes.SingleOrDefault(note =>
        note.Id == NotesContextFactory.NoteIdForDelete));
    }

    [Fact]  // Id note is not correct - so pushing error
    public async Task DeleteNoteCommandHandlerTests_FailOnWrongId()
    {
        //Arrange 
        var handler = new DeleteNoteCommandHandler(context);
        //Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        await handler.Handle(
            new DeleteNoteCommand
            {
                Id = Guid.NewGuid(),
                UserId = NotesContextFactory.UserAId,
            }, CancellationToken.None));
    }
    [Fact]
    public async Task DeleteNoteCommandHandlerTests_FailOnWrongUserId()
    {
        // Arrange
        var deleteHandler = new DeleteNoteCommandHandler(context);
        var createHandler = new CreateNoteCommandHandler(context);
        var noteId = await createHandler.Handle(new CreateNoteCommand
        {
            Title = "NoteTitle",
            Details = "NoteDetails",
            UserId = NotesContextFactory.UserAId
        }, CancellationToken.None);
        //Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
                await deleteHandler.Handle(
                    new DeleteNoteCommand
                    {
                        Id = noteId,
                        UserId = NotesContextFactory.UserBId
                    }, CancellationToken.None));
    }
}
