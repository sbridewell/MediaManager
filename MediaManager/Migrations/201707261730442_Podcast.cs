namespace SDE.MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Podcast : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PodcastChannel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Link = c.String(),
                        RssUrl = c.String(),
                        Description = c.String(),
                        Language = c.String(),
                        Copyright = c.String(),
                        LastBuildDate = c.DateTime(),
                        PublishedDate = c.DateTime(),
                        Categories = c.String(),
                        Author = c.String(),
                        OwnerName = c.String(),
                        OwnerEmail = c.String(),
                        Image_RemoteUrl = c.String(),
                        ImageTitle = c.String(),
                        ImageLink = c.String(),
                        Keywords = c.String(),
                        ITunesExplicit = c.Boolean(nullable: false),
                        Generator = c.String(),
                        Documents = c.String(),
                        ManagingEditor = c.String(),
                        Subtitle = c.String(),
                        Subscribed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PodcastEpisode",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Link = c.String(),
                        Description = c.String(),
                        Author = c.String(),
                        EnclosureUrl = c.String(),
                        EnclosureSize = c.Long(nullable: false),
                        EnclosureContentType = c.String(),
                        Guid = c.String(),
                        PublishDate = c.DateTime(),
                        Subtitle = c.String(),
                        ITunesExplicit = c.Boolean(nullable: false),
                        Duration = c.Time(nullable: false, precision: 7),
                        Image_RemoteUrl = c.String(),
                        ChannelId = c.Int(nullable: false),
                        Downloaded = c.Boolean(nullable: false),
                        Ignored = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PodcastChannel", t => t.ChannelId, cascadeDelete: true)
                .Index(t => t.ChannelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PodcastEpisode", "ChannelId", "dbo.PodcastChannel");
            DropIndex("dbo.PodcastEpisode", new[] { "ChannelId" });
            DropTable("dbo.PodcastEpisode");
            DropTable("dbo.PodcastChannel");
        }
    }
}
