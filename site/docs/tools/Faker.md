# Faker

## Overview

Tool to simulate behavior via stubs/mocks. Useful for testing in isolation from dependencies.

API Specification: <xref:Werecodent.CreateAndFake.FakerTool.IFaker>

Options: <xref:Werecodent.CreateAndFake.FakerTool.FakerOptions>

> ### Important Types
> 
> * <xref:Werecodent.CreateAndFake.FakerTool.Engine.IFakeHint> - Customizes <xref:Werecodent.CreateAndFake.FakerTool.Faker> behavior for a specific <xref:System.Type>.
> * <xref:Werecodent.CreateAndFake.FakerTool.Engine.FakePriority> - Values specifying running order for dynamically loaded hints.
> * <xref:Werecodent.CreateAndFake.FakerTool.Proxy.IFaked> - Interface for all fakes produced by the tool.

As a hint based tool, <xref:Werecodent.CreateAndFake.FakerTool.Faker> behavior provides customization via implementing an <xref:Werecodent.CreateAndFake.FakerTool.Engine.IFakeHint> class. With the default configuration, all found hints in the test assembly are loaded into the tool.

> [!NOTE]
> Faking has a dedicated documentation page here: [Fakes & Stubs & Mocks](~/docs/essentials/FakesStubsMocks.md)
