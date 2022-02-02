using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Htc.EfCoreExtensions.Tests;
public class ServicesRegistrationTests
{
    [Fact]
    public void ExtendedQueryCompiler_ShouldBeResolved()
    {
        // ARRANGE
        var sc = new ServiceCollection();
        sc.AddDbContext<TestDbContext>(o =>
        {
            o.UseInMemoryDatabase("name")
                .AddOriginalExpressionVisitors(_ => { });
        });

        var serviceProvider = sc.BuildServiceProvider();
        var ctx = serviceProvider.GetRequiredService<TestDbContext>();

        // ACT
#pragma warning disable EF1001 // Internal EF Core API usage.
        var queryCompiler = ctx.GetInfrastructure().GetService<IQueryCompiler>();
#pragma warning restore EF1001 // Internal EF Core API usage.

        // ASSERT

        queryCompiler.Should().NotBeNull();
        queryCompiler?.GetType().Should().Be(typeof(ExtendedQueryCompiler));
    }

    [Fact]
    public void VisitorsCollection_ShouldBeResolved()
    {
        // ARRANGE
        var sc = new ServiceCollection();
        sc.AddDbContext<TestDbContext>(o =>
        {
            o.UseInMemoryDatabase("name")
                .AddOriginalExpressionVisitors(_ => { });
        });

        var serviceProvider = sc.BuildServiceProvider();
        var ctx = serviceProvider.GetRequiredService<TestDbContext>();

        // ACT
        var visitorsCollection = ctx.GetInfrastructure().GetService<VisitorsCollection>();

        // ASSERT

        visitorsCollection.Should().NotBeNull();
    }

    [Fact]
    public void VisitorsCollection_ShouldBeResolvedSameAsRegistered()
    {
        // ARRANGE
        var sc = new ServiceCollection();
        sc.AddDbContext<TestDbContext>(o =>
        {
            o.UseInMemoryDatabase("name")
                .AddOriginalExpressionVisitors(b =>
                {
                    b.Add(new TestVisitor());
                    b.Add(new TestVisitor2());
                });
        });

        var serviceProvider = sc.BuildServiceProvider();
        var ctx = serviceProvider.GetRequiredService<TestDbContext>();

        var expectedVisitors = new VisitorsCollection(new List<ExpressionVisitor>
        {
            new TestVisitor(),
            new TestVisitor2()
        });

        // ACT
        var visitorsCollection = ctx.GetInfrastructure().GetService<VisitorsCollection>();

        // ASSERT

        ConvertVisitorsCollectionToTypes(visitorsCollection!).Should().BeEquivalentTo(ConvertVisitorsCollectionToTypes(expectedVisitors));
    }

    [Fact]
    public void VisitorsCollection_ShouldBeOrderedSameAsRegistered()
    {
        // ARRANGE
        var sc = new ServiceCollection();
        sc.AddDbContext<TestDbContext>(o =>
        {
            o.UseInMemoryDatabase("name")
                .AddOriginalExpressionVisitors(b =>
                {
                    b.Add(new TestVisitor());
                    b.Add(new TestVisitor2());
                });
        });

        var serviceProvider = sc.BuildServiceProvider();
        var ctx = serviceProvider.GetRequiredService<TestDbContext>();

        var expectedVisitors = new VisitorsCollection(new List<ExpressionVisitor>
        {
            new TestVisitor(),
            new TestVisitor2()
        });

        // ACT
        var visitorsCollection = ctx.GetInfrastructure().GetService<VisitorsCollection>();

        // ASSERT

        ConvertVisitorsCollectionToTypes(visitorsCollection!).Should().Equal(ConvertVisitorsCollectionToTypes(expectedVisitors));
    }

    [Fact]
    public void VisitorsCollection_ShouldBeOrderedSameAsRegistered_Negative()
    {
        // ARRANGE
        var sc = new ServiceCollection();
        sc.AddDbContext<TestDbContext>(o =>
        {
            o.UseInMemoryDatabase("name")
                .AddOriginalExpressionVisitors(b =>
                {
                    b.Add(new TestVisitor2());
                    b.Add(new TestVisitor());
                });
        });

        var serviceProvider = sc.BuildServiceProvider();
        var ctx = serviceProvider.GetRequiredService<TestDbContext>();

        var expectedVisitors = new VisitorsCollection(new List<ExpressionVisitor>
        {
            new TestVisitor(),
            new TestVisitor2()
        });

        // ACT
        var visitorsCollection = ctx.GetInfrastructure().GetService<VisitorsCollection>();

        // ASSERT

        ConvertVisitorsCollectionToTypes(visitorsCollection!).Should().NotEqual(ConvertVisitorsCollectionToTypes(expectedVisitors));
    }

    private static IEnumerable<Type> ConvertVisitorsCollectionToTypes(VisitorsCollection visitors)
    {
        return visitors.Select(x => x.GetType());
    }

    private class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {

        }
    }

    private class TestVisitor : ExpressionVisitor
    {

    }

    private class TestVisitor2 : ExpressionVisitor
    {

    }
}