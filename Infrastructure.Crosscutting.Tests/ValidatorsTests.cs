﻿////===================================================================================
//// Microsoft Developer & Platform Evangelism
////=================================================================================== 
//// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
////===================================================================================
//// Copyright (c) Microsoft Corporation.  All Rights Reserved.
//// This code is released under the terms of the MS-LPL license, 
//// http://microsoftnlayerapp.codeplex.com/license
////===================================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Crosscutting.Tests
{
    using System.Linq;
    using Infrastructure.Crosscutting.NetFramework.Validator;
    using Infrastructure.Crosscutting.Validator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass()]
    public class ValidatorsTests
    {
        #region Class Initialize

        [ClassInitialize()]
        public static void ClassInitialze(TestContext context)
        {
            // Initialize default log factory
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
        }

        #endregion


        [TestMethod()]
        public void PerformValidationIsValidReturnFalseWithInvalidEntities()
        {
            //Arrange
            var entityA = new EntityWithValidationAttribute();
            entityA.RequiredProperty = null;

            var entityB = new EntityWithValidatableObject();
            entityB.RequiredProperty = null;

            IEntityValidator entityValidator = EntityValidatorFactory.CreateValidator();

            //Act
            var entityAValidationResult = entityValidator.IsValid(entityA);
            var entityAInvalidMessages = entityValidator.GetInvalidMessages(entityA);

            var entityBValidationResult = entityValidator.IsValid(entityB);
            var entityBInvalidMessages = entityValidator.GetInvalidMessages(entityB);

            //Assert
            Assert.IsFalse(entityAValidationResult);
            Assert.IsFalse(entityBValidationResult);

            Assert.IsTrue(entityAInvalidMessages.Any());
            Assert.IsTrue(entityBInvalidMessages.Any());

        }
        [TestMethod()]
        public void PerformValidationIsValidReturnTrueWithValidEntities()
        {
            //Arrange
            var entityA = new EntityWithValidationAttribute();
            entityA.RequiredProperty = "the data";

            var entityB = new EntityWithValidatableObject();
            entityB.RequiredProperty = "the data";

            IEntityValidator entityValidator = EntityValidatorFactory.CreateValidator();

            //Act
            var entityAValidationResult = entityValidator.IsValid(entityA);
            var entityAInvalidMessages = entityValidator.GetInvalidMessages(entityA);

            var entityBValidationResult = entityValidator.IsValid(entityB);
            var entityBInvalidMessages = entityValidator.GetInvalidMessages(entityB);
          

            //Assert
            Assert.IsTrue(entityAValidationResult);
            Assert.IsTrue(entityBValidationResult);

            Assert.IsFalse(entityAInvalidMessages.Any());
            Assert.IsFalse(entityBInvalidMessages.Any());

        }
    }
    class EntityWithValidationAttribute
    {
        [Required(ErrorMessage = "This is a required property")]
        public string RequiredProperty { get; set; }
    }
    class EntityWithValidatableObject
        : IValidatableObject
    {
        public string RequiredProperty { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (String.IsNullOrEmpty(RequiredProperty)
                ||
                String.IsNullOrWhiteSpace(RequiredProperty))
            {
                validationResults.Add(new ValidationResult("Invalid Required property", new string[] { "RequiredProperty" }));
            }

            return validationResults;
        }
    }
}
