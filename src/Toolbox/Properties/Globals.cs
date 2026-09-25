#if NET9_0_OR_GREATER
global using Lock = System.Threading.Lock;
#else
global using Lock = Werecodent.CreateAndFake.Design.Content.CustomLock;
#endif
