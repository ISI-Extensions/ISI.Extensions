#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.DependencyInjection.Iunq.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class DependencyInjection_Tests
	{
		[Test]
		public void ServiceProvider_Test()
		{
			//var configuration = new ISI.Extensions.DependencyInjection.Iunq.Configuration();
			//var serviceProvider = new ISI.Extensions.DependencyInjection.Iunq.ServiceProvider(configuration);

			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			var configuration = configurationBuilder.Build();

			var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configuration);

			services.AddAllConfigurations(configuration);

			var serviceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configuration);

			serviceProvider.Register(typeof(IClassTransient), typeof(ClassTransient), Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient);

			var classTransient1 = serviceProvider.GetService<IClassTransient>();
			classTransient1.Value = Guid.NewGuid().ToString("D");

			var classTransient2 = serviceProvider.GetService<IClassTransient>();
			classTransient2.Value = Guid.NewGuid().ToString("D");

			Assert.That(!string.Equals(classTransient1.Value, classTransient2.Value, StringComparison.CurrentCulture));



			serviceProvider.Register(typeof(IClassSingleton), typeof(ClassSingleton), Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton);

			var classSingleton1 = serviceProvider.GetService<IClassSingleton>();
			classSingleton1.Value = Guid.NewGuid().ToString("D");

			var classSingleton2 = serviceProvider.GetService<IClassSingleton>();

			Assert.That(string.Equals(classSingleton1.Value, classSingleton2.Value, StringComparison.CurrentCulture));



			serviceProvider.Register(typeof(IClassScoped), typeof(ClassScoped), Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped);

			using (var serviceScopeA = serviceProvider.CreateScope())
			{
				var classScopedA1 = serviceScopeA.ServiceProvider.GetService<IClassScoped>();
				classScopedA1.Value = Guid.NewGuid().ToString("D");

				var classScopedA2 = serviceScopeA.ServiceProvider.GetService<IClassScoped>();

				Assert.That(string.Equals(classScopedA1.Value, classScopedA2.Value, StringComparison.CurrentCulture));

				using (var serviceScopeB = serviceProvider.CreateScope())
				{
					var classScopedB1 = serviceScopeB.ServiceProvider.GetService<IClassScoped>();

					Assert.That(!string.Equals(classScopedA1.Value, classScopedB1.Value, StringComparison.CurrentCulture));
				}
			}


			serviceProvider.Register(typeof(IClassComplex), typeof(ClassComplex), Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton);

			var classComplex = serviceProvider.GetService<IClassComplex>();

			Assert.That(!string.Equals(classTransient1.Value, classComplex.ClassTransient.Value, StringComparison.CurrentCulture));
			Assert.That(string.Equals(classSingleton1.Value, classComplex.ClassSingleton.Value, StringComparison.CurrentCulture));


			serviceProvider.Register(typeof(IGeneric<IClassSingleton>), typeof(ClassGeneric<IClassSingleton>), Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton);

			var classGenericA = serviceProvider.GetService<IGeneric<IClassSingleton>>();


			serviceProvider.Register(typeof(IGeneric<>), typeof(ClassGeneric<>), Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton);

			var classGenericB = serviceProvider.GetService<IGeneric<int>>();
		}


		[Test]
		public void Type_Test()
		{
			var typeA1 = typeof(IGeneric<>);
			var typeA2 = typeof(IGeneric<,>);
			var typeB1 = typeof(IGeneric<int>);


			var typeA1HashCode = typeA1.GetHashCode();
			var typeA2HashCode = typeA2.GetHashCode();
			var typeB1HashCode = typeB1.GetHashCode();

		}



		public interface IClassTransient
		{
			string Value { get; set; }
		}
		public class ClassTransient : IClassTransient
		{
			public string Value { get; set; }
		}
		public interface IClassSingleton
		{
			string Value { get; set; }
		}
		public class ClassSingleton : IClassSingleton
		{
			public string Value { get; set; }
		}
		public interface IClassScoped
		{
			string Value { get; set; }
		}
		public class ClassScoped : IClassScoped
		{
			public string Value { get; set; }
		}
		public interface IClassComplex
		{
			IClassTransient ClassTransient { get; set; }
			IClassSingleton ClassSingleton { get; set; }
		}
		public class ClassComplex : IClassComplex
		{
			public IClassTransient ClassTransient { get; set; }
			public IClassSingleton ClassSingleton { get; set; }

			public ClassComplex(
				IClassTransient classTransient,
				IClassSingleton classSingleton)
			{
				ClassTransient = classTransient;
				ClassSingleton = classSingleton;
			}
		}
		public interface IGeneric<T1>
		{
			T1 Value { get; set; }
		}
		public interface IGeneric<T1, T2>
		{
			T1 Value1 { get; set; }
			T2 Value2 { get; set; }
		}
		public class ClassGeneric<T1> : IGeneric<T1>
		{
			public T1 Value { get; set; }
		}
	}
}
