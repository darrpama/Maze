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
#	rm -rf $(DISTDIR)
#	mkdir $(DISTDIR)
#	mkdir $(DISTDIR)/src
#	cp -r $(PROJECTDIR) dvi.md Makefile $(DISTDIR)/src/
#	tar cvzf $(DISTDIR).tgz $(DISTDIR)
#	rm -rf $(DISTDIR)

check:
#	clang-format -n --style="Google" $(SOURCES) $(HEADERS)
#	cppcheck --std=c++17 --language=c++ --enable=all --suppress=unusedStructMember --suppress=missingInclude $(SOURCES) $(HEADERS)


$(STATICLIB): $(OBJECTS)
	ar rcs $(STATICLIB) $^

.PHONY: clean
clean:
	@rm -rf $(BUILDDIR)
