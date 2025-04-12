using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(l => l.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Species).IsRequired();
        builder.Property(c => c.Gender).IsRequired();

        builder.HasOne(c => c.Location)
            .WithMany(l => l.Characters)
            .HasForeignKey(c => c.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}