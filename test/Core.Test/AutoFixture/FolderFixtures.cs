﻿using System;
using AutoFixture;
using AutoFixture.Kernel;
using Bit.Core.Entities;
using Bit.Core.Test.AutoFixture.EntityFrameworkRepositoryFixtures;
using Bit.Core.Test.AutoFixture.Relays;
using Bit.Core.Test.AutoFixture.UserFixtures;
using Bit.Infrastructure.EntityFramework.Repositories;
using Bit.Test.Common.AutoFixture;
using Bit.Test.Common.AutoFixture.Attributes;

namespace Bit.Core.Test.AutoFixture.FolderFixtures
{
    internal class FolderBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var type = request as Type;
            if (type == null || type != typeof(Folder))
            {
                return new NoSpecimen();
            }

            var fixture = new Fixture();
            fixture.Customizations.Insert(0, new MaxLengthStringRelay());
            var obj = fixture.WithAutoNSubstitutions().Create<Folder>();
            return obj;
        }
    }

    internal class EfFolder : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreVirtualMembersCustomization());
            fixture.Customizations.Add(new GlobalSettingsBuilder());
            fixture.Customizations.Add(new FolderBuilder());
            fixture.Customizations.Add(new UserBuilder());
            fixture.Customizations.Add(new EfRepositoryListBuilder<FolderRepository>());
            fixture.Customizations.Add(new EfRepositoryListBuilder<UserRepository>());
        }
    }

    internal class EfFolderAutoDataAttribute : CustomAutoDataAttribute
    {
        public EfFolderAutoDataAttribute() : base(new SutProviderCustomization(), new EfFolder())
        { }
    }

    internal class InlineEfFolderAutoDataAttribute : InlineCustomAutoDataAttribute
    {
        public InlineEfFolderAutoDataAttribute(params object[] values) : base(new[] { typeof(SutProviderCustomization),
            typeof(EfFolder) }, values)
        { }
    }
}

