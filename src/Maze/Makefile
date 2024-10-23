PROJECTDIR = ./
BUILDDIR = ../mazebuild
DISTDIR = $(BUILDDIR)/GuiInterface/bin/Release/*/*/publish/
INSTALLDIR = $(PROJECTDIR)
EXECUTABLE = Maze

all: test

test: build
	cd $(BUILDDIR)/MazeTests && dotnet test
	echo $(DISTDIR)

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

dist:

check:


$(STATICLIB): $(OBJECTS)
	ar rcs $(STATICLIB) $^

.PHONY: clean
clean:
	@rm -rf $(BUILDDIR)
