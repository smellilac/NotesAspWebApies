﻿using AutoMapper;
using Notes.Application;
using Notes.Application.Common.Mapping;
using Notes.WebApi.PersistanceLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Common;

public class QueryTestFixture : IDisposable
{
    public NotesDbContext Context;
    public IMapper Mapper;

    public QueryTestFixture()
    {
        Context = NotesContextFactory.Create();
        var configurationProvider = new MapperConfiguration(config =>
        {
            config.AddProfile(new AssemblyMappingProfile
                (typeof(INotesDbContext).Assembly)); 
        });
        Mapper = configurationProvider.CreateMapper();
    }

    public void Dispose()
    {
        NotesContextFactory.Destroy(Context);
    }

}
[CollectionDefinition("QueryCollection")]
public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
