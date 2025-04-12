using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .ValueGeneratedNever();

        builder.Property(l => l.Name).IsRequired();
        builder.Property(l => l.Type).IsRequired();

        // Index on Location Name to optimize querying by planet name
        builder.HasIndex(l => l.Name).HasDatabaseName("IX_Location_Name");
    }
}