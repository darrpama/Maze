PROJECTDIR = ./
BUILDDIR = ../mazebuild
DISTDIR = $(BUILDDIR)/GuiInterface/bin/Release/*/*/publish/
TMPDIR = ../todist
INSTALLDIR = $(PROJECTDIR)
EXECUTABLE = Maze

all: test

test:
	cd $(BUILDDIR)/MazeTests && sudo -S dotnet test

dvi:
	open ./dvi.md

build: clean
	cd $(PROJECTDIR) && ls -la && mkdir $(BUILDDIR) && cp -r $(PROJECTDIR) $(BUILDDIR)\
	&& cd $(BUILDDIR) && dotnet publish GuiInterface

install: build
	cd $(DISTDIR) && ls -la 
	mv $(DISTDIR)GuiInterface $(INSTALLDIR)$(EXECUTABLE)

uninstall:
	cd $(INSTALLDIR) && ls -la && rm $(INSTALLDIR)$(EXECUTABLE)

dist: clean
	mkdir $(TMPDIR)
	cp -r $(PROJECTDIR) $(TMPDIR)
	cd .. && tar -czvf distributive.tgz todist
	rm -rf $(TMPDIR)


.PHONY: clean
clean:
	@rm -rf $(BUILDDIR)
	@rm -rf $(TMPDIR)
