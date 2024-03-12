using Microsoft.EntityFrameworkCore;
using Notes.Application.Commands.DeleteNote;
using Notes.Application.Commands.UpdateNote;
using Notes.Application.Common.Exceptions;
using Notes.Domain;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Commands;

public class UpdateNoteCommandHandlerTests : TestCommandBase
{
    [Fact]// Успешное обновление заметки
    public async Task UpdateNoteCommandHandlerTests_Succes()
    {
        // Arrange 
        var handler = new UpdateNoteCommandHandler(context);
        var updatedTitle = " new title";
        //Act
        await handler.Handle(new UpdateNoteCommand
        {
            Id = NotesContextFactory.NoteIdForUpdate,
            UserId = NotesContextFactory.UserBId,
            Title = updatedTitle,
        }, CancellationToken.None);
        // Assert
        Assert.NotNull(await context.Notes.SingleOrDefaultAsync(note =>
        note.Id == NotesContextFactory.NoteIdForUpdate &&
        note.Title == updatedTitle));
    }
    [Fact]// неправильное id заметки 
    public async Task UpdateNoteCommandHandlerTests_FailOnWrongId()
    {
        // Arrange 
        var handler = new UpdateNoteCommandHandler(context);
        //Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        await handler.Handle(new UpdateNoteCommand
        {
            Id = Guid.NewGuid(),
            UserId = NotesContextFactory.UserAId,
        }, CancellationToken.None));
    }
    [Fact]
    public async Task UpdateNoteCommandHandler_FailOnWrongUserId()
    {
        // Arrange
        var handler = new UpdateNoteCommandHandler(context);

        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        await handler.Handle(new UpdateNoteCommand
        {
            Id = NotesContextFactory.NoteIdForUpdate,
            UserId =NotesContextFactory.UserAId,
        },  CancellationToken.None));
    }
}