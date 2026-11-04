---
title: Starting page
date: 2026-11-04 12:00:00 +0000
categories: [Help, Instructions]
tags: [getting started]
pin: true
---

# Getting Started

Welcome to the documentation source for developers. 

## Quick Start

A default set of all tools are ready to use under the static class Tools.

```c#
/// <summary>Verifies that the tools integrate together.</summary>
[Fact]
public void Tools_IntegrationWorks()
{
    DataSample original = Tools.Randomizer.Create<DataSample>();
    DataSample variant = Tools.Mutator.Variant(original);
    DataSample dupe = Tools.Duplicator.Copy(original);

    Tools.Asserter.Is(original, dupe);
    Tools.Asserter.IsNot(original, variant);

    Fake<DataSample> faked = Tools.Faker.Mock<DataSample>();
    faked.Setup(
        d => d.HasNested(dupe),
        Behavior.Returns(true, Times.Once));

    Tools.Asserter.Is(true, faked.Dummy.HasNested(original),
        "Value equality did not work for args.");

    faked.Verify(Times.Once);
}
```

The tools are designed to cover many common scenarios including immutable classes, classes with private fields, and classes with static factories instead of constructors.

## Customization

In case the default behavior is not enough, each tool can be customized for individual needs by creating your own instances. Additionally, interfaces have been provided for data classes to properly behave with the tools instead. Visit any tool's page to be guided on either route.

Although each tool can be created and customized on a per test basis, the global tools can be modified via a 'ModuleInitializer':

```c#
[ModuleInitializer]
internal static void ModifyTools()
{
    Tools.Source = ToolSet.CreateViaSeed(100);

    Tools.Randomizer.AddHint(new StringCreateHint());
}
```

This way, any customization changes are picked up throughout your code.

<h2>General:</h2>
<ul>
    {% for page in site.html_pages %}
    {% unless page.url contains 'Design' %}
    {% unless page.url contains 'Tools' %}
    <li>
        <h3>
            <a href="{{ site.baseurl}}{{ page.url }}">{{page.title}}</a>
        </h3>
    </li>
    {% endunless %}
    {% endunless %}
    {% endfor %}
</ul>
<h2>Design:</h2>
<ul>
    {% for page in site.html_pages %}
    {% if page.url contains 'Design' %}
    <li>
        <h3>
            <a href="{{ site.baseurl}}{{ page.url }}">{{page.title}}</a>
        </h3>
    </li>
    {% endif %}
    {% endfor %}
</ul>
<h2>Tools:</h2>
<ul>
    {% for page in site.html_pages %}
    {% if page.url contains 'Tools' %}
    <li>
        <h3>
            <a href="{{ site.baseurl}}{{ page.url }}">{{page.title}}</a>
        </h3>
    </li>
    {% endif %}
    {% endfor %}
</ul>
