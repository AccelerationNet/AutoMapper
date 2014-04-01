using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
        }
    }

    [TestClass]
    public class FlattenChildCollection {
        class Result {
            public ICollection<string> Roles { get; set; }
        }

        class Role {
            public string Name { get; set; }
        }
        class User {
            public ICollection<Role> Roles { get; set; }
        }


        public FlattenChildCollection() {
            Mapper.CreateMap<User, Result>()
                // map the name off the roles
                .ForMember(
                    d => d.Roles,
                    cfg => cfg.MapFrom(s => s.Roles.Select(x => x.Name)));
        }

        [TestMethod]
        public void RolesGetMapped() {
            var user = new User {
                Roles = new[]{
                    new Role{ Name="A"},
                    new Role{ Name="B"},
                }
            };

            var users = (new[] { user }).AsQueryable();

            var result = users.Project().To<Result>().Single();
            var roles = result.Roles.ToList();
            Assert.AreEqual(2, roles.Count);
            Assert.IsTrue(roles.Contains("A"));
            Assert.IsTrue(roles.Contains("B"));
        }


    }
}
