# Extractor

## Overview

Tool to flatten the contents of objects. Useful for determining what data is shared among objects.

API Specification: <xref:Werecodent.CreateAndFake.ExtractorTool.IExtractor>

Options: <xref:Werecodent.CreateAndFake.ExtractorTool.ExtractorOptions>

> ### Important Types
> 
> * <xref:Werecodent.CreateAndFake.ExtractorTool.Engine.IExtractHint> - Customizes <xref:Werecodent.CreateAndFake.ExtractorTool.Extractor> behavior for a specific <xref:System.Type>.
> * <xref:Werecodent.CreateAndFake.ExtractorTool.Engine.ExtractPriority> - Values specifying running order for dynamically loaded hints.

As a hint based tool, <xref:Werecodent.CreateAndFake.ExtractorTool.Extractor> behavior provides customization via implementing an <xref:Werecodent.CreateAndFake.ExtractorTool.Engine.IExtractHint> class. With the default configuration, all found hints in the test assembly are loaded into the tool.
