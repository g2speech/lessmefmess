# Less MEF mess

If you ever got a 1 500 lines long CompositionError from MEF, you will appreciate that this tool can draw out the dependencies in a 
graph for you. It depends on the free as in free beer yEd graph editor from yWorks being installed on your computer.

You just start this application:

![startup screen](screenshots/startup.png?raw=true "Startup screen")

Paste some mess that MEF burped up into it:

![after the paste](screenshots/adding_some_mess.png?raw=true "Added 1 500 lines of CompositionError")

Press the button on the bottom of the app. If you are lucky (say, yEd is installed in "C:\Program Files (x86)\yEd\yEd.exe"), the
graph opens up in yEd, but everything is squeezed into 1 point. Just choose Layout -> Radial (Alt+Shift+A), go with the defaults and
press OK. The only problem now is that your labels do not fit the nodes. Just choose Tools -> Fit Node to Label (no shortcut) and
rejoice.

![a poisonous flower](screenshots/graphed_out.png?raw=true "Finally yEd shows you the dependencies")

Now you can zoom in, out, use the neighborhood view to understand what went belly-up in your container. The only thing left for you is
to fix your program.