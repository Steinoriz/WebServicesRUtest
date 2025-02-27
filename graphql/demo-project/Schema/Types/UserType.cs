﻿using demo_project.Data;
using demo_project.Models;
using GraphQL.Types;

namespace demo_project.Schema.Types;

public sealed class UserType : ObjectGraphType<User>
{
    public UserType(DemoData data)
    {
        Field(x => x.Id).Description("The id of the user");
        Field(x => x.Name).Description("The name of the user");

        Field<ListGraphType<PhotoType>>(
            "postedPhotos",
            "The photos this user has posted",
            resolve: context => data.Photos.Where(p => p.UserId == context.Source.Id)
        );

        Field<ListGraphType<PhotoType>>(
            "inPhotos",
            description: "The photos this user is tagged in",
            resolve: context =>
            {
                var userId = context.Source.Id;
                var photoIds = data.Tags.Where(t => t.UserId == userId).Select(t => t.PhotoId);
                return data.Photos.Where(p => photoIds.Contains(p.Id));
            }
        );
    }
}