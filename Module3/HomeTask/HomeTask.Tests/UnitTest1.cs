using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeTask.Tests
{
    [TestFixture]
    public class FileSystemVisitorTests
    {
        private Mock<IDirectoryWrapper> _directoryMock;
        private FileSystemVisitor _visitor;

        [SetUp]
        public void SetUp()
        {
            _directoryMock = new Mock<IDirectoryWrapper>();
        }

        [TearDown]
        public void TearDown()
        {
            _visitor = null;
        }

        [Test]
        public void Read_NormalScenario_ShouldReturnAllFilesAndDirectories()
        {
            _visitor = new FileSystemVisitor("C:\\TestPath",directoryWrapper: _directoryMock.Object);


            _directoryMock.Setup(d => d.EnumerateFileSystemEntries(It.IsAny<string>()))
                          .Returns(new List<string>
                          {
                              "C:\\TestPath\\file1.txt",
                              "C:\\TestPath\\file2.log",
                              "C:\\TestPath\\subdir1",
                          });

            var entries = _visitor.Read().ToList();

            Assert.That(entries.Count, Is.EqualTo(3));
            Assert.That(entries, Does.Contain("C:\\TestPath\\file1.txt"));
            Assert.That(entries, Does.Contain("C:\\TestPath\\file2.log"));
            Assert.That(entries, Does.Contain("C:\\TestPath\\subdir1"));
        }

        [Test]
        public void Read_WithFilters_ShouldReturnFilteredFilesAndDirectories()
        {
            _visitor = new FileSystemVisitor("C:\\TestPath", path => path.EndsWith(".txt"), _directoryMock.Object);


            _directoryMock.Setup(d => d.EnumerateFileSystemEntries(It.IsAny<string>()))
                          .Returns(new List<string>
                          {
                              "C:\\TestPath\\file1.log",
                              "C:\\TestPath\\file2.txt",
                              "C:\\TestPath\\subdir1",
                          });

            var entries = _visitor.Read().ToList();

            Assert.That(entries.Count, Is.EqualTo(1));
            Assert.That(entries, Does.Contain("C:\\TestPath\\file2.txt"));
        }

        [Test]
        public void Read_AbortIsTrue_ShouldAbortSearch()
        {
            _visitor = new FileSystemVisitor("C:\\TestPath", path => path.EndsWith(".txt"), _directoryMock.Object);


            _directoryMock.Setup(d => d.EnumerateFileSystemEntries(It.IsAny<string>()))
                          .Returns(new List<string>
                          {
                              "C:\\TestPath\\file1.txt",
                              "C:\\TestPath\\file2.log",
                              "C:\\TestPath\\subdir1",
                              "C:\\TestPath\\subdir1\\file3.txt"
                          });

            _visitor.FileFound += (sender, e) => e.AbortSearch = true;

            var entries = _visitor.Read().ToList();

            Assert.That(entries.Count, Is.EqualTo(0));
        }

        [Test]
        public void Read_ExcludeIsTrue_ShouldExcludeFilesAndDirectories()
        {
            _visitor = new FileSystemVisitor("C:\\TestPath", path => path.EndsWith(".txt"), _directoryMock.Object);


            _directoryMock.Setup(d => d.EnumerateFileSystemEntries(It.IsAny<string>()))
                          .Returns(new List<string>
                          {
                              "C:\\TestPath\\file1.txt",
                              "C:\\TestPath\\file2.log",
                              "C:\\TestPath\\subdir1",
                          });

            _visitor.FileFound += (sender, e) => { if (e.Path.EndsWith(".log")) e.Exclude = true; };
            _visitor.DirectoryFound += (sender, e) => { if (e.Path.EndsWith("subdir1")) e.Exclude = true; };

            var entries = _visitor.Read().ToList();

            Assert.That(entries.Count, Is.EqualTo(1));
            Assert.That(entries, Does.Contain("C:\\TestPath\\file1.txt"));
        }
    }
}
