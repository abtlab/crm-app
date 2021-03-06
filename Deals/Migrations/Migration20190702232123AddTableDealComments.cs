﻿using FluentMigrator;

namespace Crm.Apps.Deals.Migrations
{
    [Migration(20190702232123)]
    public class Migration20190702232123AddTableDealComments : Migration
    {
        public override void Up()
        {
            Create.Table("DealComments")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("DealId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_DealComments_Id").OnTable("DealComments")
                .Column("Id");

            Create.Index("IX_DealComments_DealId_CreateDateTime").OnTable("DealComments")
                .OnColumn("DealId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealComments_DealId_CommentatorUserId_Value_CreateDateTime").OnTable("DealComments");
            Delete.PrimaryKey("PK_DealComments_Id").FromTable("DealComments");
            Delete.Table("DealComments");
        }
    }
}