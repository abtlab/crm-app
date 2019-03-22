﻿using FluentMigrator;

namespace Crm.Areas.Accounts.Migrations
{
    [Migration(20190313000217)]
    public class Migration20190313000217AddTableAccountSettings : Migration
    {
        public override void Up()
        {
            Create.Table("AccountSettings")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_AccountSettings_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Value").AsString().NotNullable();

            Create.UniqueConstraint("UQ_AccountSettings_AccountId_Type").OnTable("AccountSettings")
                .Columns("AccountId", "Type");

            Create.Index("IX_AccountSettings_AccountId_Type").OnTable("AccountSettings")
                .OnColumn("AccountId").Descending()
                .OnColumn("Type").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_AccountSettings_AccountId_Type").OnTable("AccountSettings");

            Delete.UniqueConstraint("UQ_AccountSettings_AccountId_Type").FromTable("AccountSettings");

            Delete.Table("AccountSettings");
        }
    }
}