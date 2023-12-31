﻿using TP_CS.Business.IRepositories;
using TP_CS.Business.Models;
using TP_CS.Data.Context;

namespace TP_CS.Data.Repositories;

public class DatabaseTagRepository : ITagRepository
{
    private readonly MyDbContext _dbContext;

    public DatabaseTagRepository(MyDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IEnumerable<Tag>? GetTags()
    {
        return _dbContext.Tags?.ToList();
    }

    public Tag GetTagById(long id)
    {
        return _dbContext.Tags?.FirstOrDefault(t => t.Id == id)!;
    }

    public void CreateTag(Tag newtag, UserTask task)
    {
        _dbContext.Tasks?.FirstOrDefault(t => t.Id == task.Id)?.Tags.Add(newtag);
        _dbContext.Tags?.Add(newtag);
        _dbContext.SaveChanges();
    }

    public BusinessResult<Tag> UpdateTag(Tag tag, long id)
    {
        var existingTeam = _dbContext.Tags?.FirstOrDefault(t => t.Id == id);

        if (existingTeam != null)
        {
            existingTeam.Name = tag.Name;
            existingTeam.Color = tag.Color;
            existingTeam.Description = tag.Description;
            existingTeam.Iscomplete = tag.Iscomplete;
        }
        else
        {
            throw new InvalidOperationException("Etiquette introuvable");
        }
        _dbContext.SaveChanges();
        return  BusinessResult<Tag>.FromSuccess(existingTeam);
    }

    public void DeleteTag(long id)
    {
        var tag = _dbContext.Tags?.FirstOrDefault(t => t.Id == id);
        if (tag == null)
        {
            throw new InvalidOperationException("Étiquette introuvable");
        }

        _dbContext.Tags?.Remove(tag);
        _dbContext.SaveChanges();
    }

    public IEnumerable<Tag>? SearchTag(string keyword)
    {
        return _dbContext.Tags?.AsEnumerable()
            .Where(tag => tag.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}