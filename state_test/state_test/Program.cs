using System;
using System.Collections.Generic;

namespace state_test
{
    class Program
    {
        static void Main(string[] args)
        {
            State prog=new MenuState();
            prog.Go();

        }
    }



    class State
    {
	    protected bool looping=true;

	    public virtual void Create()
	    {
        }

	    public virtual void Clear()
	    {
        }

	    public virtual void UpdateInput(float time)
	    {
        }

	    public virtual void Update(float time)
	    {
	    }

	    public virtual void Go()
	    {
            looping=true;
            Create();

		    //gameloop
            while(looping)
            {
                UpdateInput(0.1f);
                Update(0.1f);

                System.Threading.Thread.Sleep(500);
                Console.Write(".");
                // swap ym kun grafiikkaa
            }
            
            Clear();
	    }
    }

    class MenuState : State
    {
        public override void Create()
        {
            Console.WriteLine("create menu");
            base.Create();
        }
        public override void Clear()
        {
            Console.WriteLine("clear menu");
            base.Clear();
        }
        public override void Update(float time)
        {
            Console.WriteLine("Menu:\n1..aloitus\n2..ohje\n3..exit");
            String str = Console.ReadLine();
            if (str == null || str.Length == 0) return;

            switch (str[0])
            {
                case '1':
                    State game = new GameState();
                    game.Go();
                    break;

                case '2':
                    State help = new HelpState();
                    help.Go();
                    break;
                
                case '3':
                    looping = false;
                    break;
            }
        }
    }

    class HelpState : State
    {
        public override void Create()
        {
            Console.WriteLine("create help");
            base.Create();
        }
        public override void Clear()
        {
            Console.WriteLine("clear help");
            base.Clear();
        }
        public override void Update(float time)
        {
            Console.WriteLine("render help.. enter to return");
            Console.ReadLine();
            looping = false;
        }
    }

    class GameState : State
    {
        public override void Create()
        {
            Console.WriteLine("create game");
            base.Create();
        }
        public override void Clear()
        {
            Console.WriteLine("clear game");
            base.Clear();
        }
        public override void Update(float time)
        {
            Console.WriteLine("gaming.. Q == exit to menu");
            String str = Console.ReadLine();
            if (str == null || str.Length == 0) return;

            switch (str[0])
            {
                case 'Q':
                    looping = false;
                    break;
            }
        }
    }



}
