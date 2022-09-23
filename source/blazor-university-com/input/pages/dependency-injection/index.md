---
title: "Dependency injection"
date: "2019-04-27"
order: 9
---

## Overview of dependency injection

Dependency injection is a best-practice software development technique for ensuring classes remain loosely coupled and
making unit testing easier.

Take, for example, a service that uses a 3rd party service for sending emails.
Traditionally, any class needing to use this service might create an instance.

```cs
public class NewsletterService
{
  private readonly IEmailService EmailService;

  public NewsletterService()
  {
    EmailService = new SendGridEmailService();
  }

  public void SignUp(string emailAddress)
  {
     EmailService.SendEmail("noreply@sender.com", emailAddress, "Subject", "Body");
  }
}
```

The problem with this approach is that it tightly couples the classes `NewsletterService` and `SendGridEmailService`.
When unit testing `MyClass.SignUp`, the method being tested would actually try to send an email.
Not only is this not so good for your inbox, it's not good for costs (if your provider charges per email),
and gives more points at which your test can fail when in fact all we need to know is that the `SignUp` method attempts
to send out emails to welcome new users to our service.

Using dependency injection instead of every consuming class having to create an instance of the correct `IEmailService` implementor,
our consuming classes expect the correct instance to be supplied when it is created.

```cs
public class NewsletterService
{
  private readonly IEmailService EmailService;

  public NewsletterService(IEmailService emailService)
  {
    EmailService = emailService;
  }

  public void SignUp(string emailAddress)
  {
     EmailService.SendEmail(...);
  }
}
```

A Dependency Injection Framework (such as the one used by default in ASP.NET MVC apps and Blazor apps) will automatically
inject an instance of the correct class when we ask it to build up an instance of `NewsletterService` for us.

Not only does this decouple our classes by making `NewsletterService` unaware of the class that implements `IEmailService`,
but it also makes unit-testing very simple. For example, using the Moq framework.

```cs
[Fact]
public void WhenSigningUp\_ThenSendsAnEmail()
{
  var mockEmailService = new Mock<IEmailService>();
  
  var subject = new NewsletterService(mockEmailService.Object);
  subject.SignUp("Bob@Monkhouse.com");

  mockEmailService
    .Verify(
      x => x.Send("noreply@sender.com", "Bob@Monkhouse.com", "Subject", "Body"),
      Times.Once);
}
```
