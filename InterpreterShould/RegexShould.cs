using System;
using System.Text.RegularExpressions;
using JavaScriptInterpreter;
using Xunit;
using Xunit.Abstractions;

namespace InterpreterShould
{
  [Collection("Regex collection")]
  public class RegexShould
  {

    Regex regexIsValidFileName = new Regex(@"\d*.jpg");

    [Fact]
    public void RegexSimpleNumJpg()
    {
      string testString = "1.jpg";

      bool result = regexIsValidFileName.IsMatch(testString);
      Assert.True(result);
    }
    [Fact]
    public void RegexBigNumJpg()
    {
      string testString = "302.jpg";
      bool result = regexIsValidFileName.IsMatch(testString);
      Assert.True(result);
    }
    [Fact]
    public void RegexNumPng()
    {
      string testString = "1.png";
      bool result = regexIsValidFileName.IsMatch(testString);
      Assert.False(result);
    }
    [Fact]
    public void RegexSmallNumNoFileType()
    {
      string testString = "1";
      bool result = regexIsValidFileName.IsMatch(testString);
      Assert.False(result);
    }
    [Fact]
    public void RegexLargeNumPng()
    {
      string testString = "302.png";
      bool result = regexIsValidFileName.IsMatch(testString);
      Assert.False(result);
    }
  }
}
