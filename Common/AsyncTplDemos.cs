using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsynPTLDemos
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //What is a Task?

//            SimpleTaskExample();

//            SimpleTaskExmapleWithCorrectCreation();

            //SimpleTaskExmapleWithReturnValue();

            SimpleTaskWithException();

            //SimpleTaskWithExceptionHandle();

//            SimpleTaskWithExceptionHandleOther();

            //SimpleTaskWithExceptionHandleOther();

//            SimpleTaskExampleWithCancelation();

            //SimpleTaskExampleWithUIContinueWith();

//            SimpleTaskExampleWithWhenAny();

            //SimpleTaskExampleWithWhenAll();

            //ComplexTaskExampleWithWhenAll();

            //ParallelForAndForEach();

            //ParallelForAndForEachWithStopAndBreak();

            //ParallelForAndForEachWithThreadLocalStorage();


            //ignore just to jump to async


            //var task = SimpleAsync();

            //var task = SimpleAsyncWaitAll();

            //var task = SimpleAsyncWhenAny();


            //var task = SimpleAsyncWhenAnyWithCancelationToken();

            //task.Wait();


            Console.ReadLine();


            // Extra Reading
            //http://www.codeproject.com/Articles/152765/Task-Parallel-Library-of-n
            //http://anthonysteele.co.uk/async-and-await-with-console-apps#sthash.ROay4V1D.dpuf
            //https://msdn.microsoft.com/en-us/magazine/jj991977.aspx
        }

        private static async Task SimpleAsyncWhenAwaitablen()
        {
            await SimpleAsync().ConfigureAwait(false);

            //httpcontext

            Console.WriteLine("End Tasks");
        }


        private static async Task SimpleAsyncWhenAnyWithCancelationToken()
        {
            // create the cancellation token source
            var tokenSource = new CancellationTokenSource();
            // create the cancellation token
            var token = tokenSource.Token;

            var listOfTasks = new List<Task>
                              {
                                  SimpleAsync(token),
                                  SimpleAsync(token),
                                  SimpleAsync(token)
                              };

            await Task.WhenAny(listOfTasks);
            tokenSource.Cancel();
            Console.WriteLine("End Tasks");
        }

        private static async Task SimpleAsync(CancellationToken token)
        {
            var answer = await AddWithTask(40, 2);
            token.ThrowIfCancellationRequested();
            Console.WriteLine("The answer is " + answer);
        }


        private static async Task SimpleAsyncWhenAny()
        {
            var listOfTasks = new List<Task>
                              {
                                  SimpleAsync(),
                                  SimpleAsync(),
                                  SimpleAsync()
                              };

            await Task.WhenAny(listOfTasks);

            Console.WriteLine("End Tasks");
        }

        private static async Task SimpleAsyncWaitAll()
        {
            var listOfTasks = new List<Task>
                              {
                                  SimpleAsync(),
                                  SimpleAsync(),
                                  SimpleAsync()
                              };

            await Task.WhenAll(listOfTasks);

            Console.WriteLine("End Tasks");
        }

        private static async Task SimpleAsync()
        {
            var answer = await AddWithTask(40, 2);
            Console.WriteLine("The answer is " + answer);
        }

        private static async Task<int> AddWithTask(int a, int b)
        {
            return await Task.Factory.StartNew(() =>
                                               {
                                                   Thread.Sleep(2000);
                                                   return a + b;
                                               });
        }


        //
        // Summary:
        //     Executes a for each operation on an System.Collections.IEnumerable{TSource}
        //     in which iterations may run in parallel.
        //
        // Parameters:
        //   source:
        //     An enumerable data source.
        //
        //   localInit:
        //     The function delegate that returns the initial state of the local data for
        //     each thread.
        //
        //   body:
        //     The delegate that is invoked once per iteration.
        //
        //   localFinally:
        //     The delegate that performs a final action on the local state of each thread.
        //
        // Type parameters:
        //   TSource:
        //     The type of the data in the source.
        //
        //   TLocal:
        //     The type of the thread-local data.
        //
        // Returns:
        //     A System.Threading.Tasks.ParallelLoopResult structure that contains information
        //     on what portion of the loop completed.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The exception that is thrown when the source argument is null.-or-The exception
        //     that is thrown when the body argument is null.-or-The exception that is thrown
        //     when the localInit argument is null.-or-The exception that is thrown when
        //     the localFinally argument is null.
        private static void ParallelForAndForEachWithThreadLocalStorage()
        {
            var matches = 0;
            var syncLock = new object();

            string[] words
                =
            {
                "the", "other", "day", "I", "was",
                "speaking", "to", "a", "man", "about",
                "this", "and", "that", "cat", "and", "he", "told", "me",
                "the", "only", "other", "person", "to", "ask", "him",
                "about", "the", "crazy", "cat", "was", "the", "cats", "owner"
            };

            var searchWord = "cat";

            //Add up all words that match the searchWord
            Parallel.ForEach(
                //source
                words,
                //local init
                () => 0,
                //body
                (string item, ParallelLoopState loopState, int tlsValue) =>
                {
                    if (item.ToLower().Equals(searchWord))
                    {
                        tlsValue++;
                    }
                    return tlsValue;
                },
                //local finally
                tlsValue =>
                {
                    lock (syncLock)
                    {
                        matches += tlsValue;
                    }
                });

            Console.WriteLine("Matches for searchword '{0}' : {1}\r\n", searchWord, matches);
            Console.WriteLine("Where the original word list was : \r\n\r\n{0}",
                words.Aggregate((x, y) => x.ToString() + " " + y.ToString()));
        }

        private static void ParallelForAndForEachWithStopAndBreak()
        {
            var data = new List<string> {"There", "were", "many", "animals", "at", "the", "zoo"};

            //parallel for stop
            var res1 = Parallel.For(0,
                10,
                (x, state) =>
                {
                    if (x < 5)
                        Console.WriteLine(x);
                    else
                        state.Stop();
                });

            Console.WriteLine("For loop LowestBreak Iteration : {0}",
                res1.LowestBreakIteration);
            Console.WriteLine("For loop Completed : {0}", res1.IsCompleted);
            Console.WriteLine("\r\n");


            //parallel foreach stop
            var res2 = Parallel.ForEach(data,
                (x, state) =>
                {
                    if (!x.Equals("zoo"))
                        Console.WriteLine(x);
                    else
                        state.Stop();
                });
            Console.WriteLine("Foreach loop LowestBreak Iteration : {0}",
                res2.LowestBreakIteration);
            Console.WriteLine("Foreach loop Completed : {0}", res2.IsCompleted);
            Console.WriteLine("\r\n");


            //parallel for each that actuaally breaks, rather than stops
            var res3 = Parallel.ForEach(data,
                (x, state) =>
                {
                    if (x.Equals("zoo"))
                    {
                        Console.WriteLine(x);
                        state.Break();
                    }
                });
            Console.WriteLine("Foreach loop LowestBreak Iteration : {0}",
                res3.LowestBreakIteration);
            Console.WriteLine("Foreach loop Completed : {0}", res3.IsCompleted);
        }

        private static void ParallelForAndForEach()
        {
            var data = new List<string> {"There", "were", "many", "animals", "at", "the", "zoo"};

            //parallel for
            Parallel.For(0, 10, x => { Console.WriteLine(x); });

            //parallel for each
            Parallel.ForEach(data, x => { Console.WriteLine(x); });

            Console.ReadLine();
        }

        private static void ComplexTaskExampleWithWhenAll()
        {
            //create 3 tasks 
            var tasks = new Task<string>[3];

            tasks[0] = Task.Factory.StartNew(() => Action(1000, "Test1"));

            tasks[0].ContinueWith(task => { Console.WriteLine("Tag on extra work"); }, TaskContinuationOptions.OnlyOnRanToCompletion);


            tasks[1] = Task.Factory.StartNew(() => Action(2000, "Test2"));

            tasks[1].ContinueWith(task => { Console.WriteLine("Tag on extra work is Faulted"); }, TaskContinuationOptions.OnlyOnFaulted);

            tasks[2] = Task.Factory.StartNew(() => Action(3000, "Test3"));


            //Wait for any of them (assuming nothing goes wrong)
            Task.Factory.ContinueWhenAll(tasks,
                antecedents =>
                {
                    foreach (var task in antecedents)
                    {
                        Console.WriteLine(task.Result.ToString());
                    }
                });
        }

        private static void SimpleTaskExampleWithWhenAll()
        {
            //create 3 tasks 
            var tasks = new Task<string>[3];

            tasks[0] = Task.Factory.StartNew(() => Action(1000, "Test1"));
            tasks[1] = Task.Factory.StartNew(() => Action(2000, "Test2"));
            tasks[2] = Task.Factory.StartNew(() => Action(3000, "Test3"));


            //Wait for any of them (assuming nothing goes wrong)
            Task.Factory.ContinueWhenAll(tasks,
                antecedents =>
                {
                    foreach (var task in antecedents)
                    {
                        Console.WriteLine(task.Result.ToString());
                    }
                });
        }

        private static void SimpleTaskExampleWithWhenAny()
        {
            //create 3 tasks 
            var tasks = new Task<string>[3];

            tasks[0] = Task.Factory.StartNew(() => Action(1000, "Test1"));
            tasks[1] = Task.Factory.StartNew(() => Action(2000, "Test2"));
            tasks[2] = Task.Factory.StartNew(() => Action(3000, "Test3"));


            //Wait for any of them (assuming nothing goes wrong)
            Task.Factory.ContinueWhenAny(tasks, firstTask => { Console.WriteLine(firstTask.Result); });
        }

        private static string Action(int waitTime, string name)
        {
            Console.WriteLine("Task Method:" + name);

            Thread.Sleep(waitTime);

            return "Test String:" + name;
        }

        private static void SimpleTaskExampleWithUIContinueWith()
        {
            Console.WriteLine("Start Method");


            // Create a task and start
            var t1 = Task.Factory.StartNew(() => { Console.WriteLine("Hello from Task"); }).ContinueWith(ant =>
                                                                                                         {
                                                                                                             //updates UI no problem as we are using correct SynchronizationContext

                                                                                                             Console.WriteLine("Hello from ContinueWith");
                                                                                                         },
                TaskScheduler.Current);

            //TaskScheduler.FromCurrentSynchronizationContext();


            t1.Wait();

            Console.WriteLine("End Method");
        }

        private static void SimpleTaskExampleWithCancelation()
        {
            Console.WriteLine("Start Method");


            // create the cancellation token source
            var tokenSource = new CancellationTokenSource();
            // create the cancellation token
            var token = tokenSource.Token;

            // Create a task and start
            var t1 = Task.Factory.StartNew(() => SomeMethod(token), token);

            Thread.Sleep(2000);
            tokenSource.Cancel();
            try
            {
                Console.WriteLine(t1.Result);
            }
            catch (AggregateException aggEx)
            {
                foreach (var ex in aggEx.InnerExceptions)
                {
                    Console.WriteLine("Caught exception '{0}'", ex.Message);
                }
            }

            Console.WriteLine("End Method");
        }

        private static string SomeMethod(CancellationToken token)
        {
            Console.WriteLine("Task Method");

            Thread.Sleep(3000);

            token.ThrowIfCancellationRequested();

            return "Test String";
        }

        private static void SimpleTaskWithExceptionHandleOther()
        {
            Console.WriteLine("Start Method");

            // Create a task and start, use this 95% of the time. Use .Start() if you need to schedule a continuation within the task action.
            var t1 = Task.Factory.StartNew(() =>
                                           {
                                               throw new Exception("Exception Message !!!");

                                               return "Hello from Task";
                                           });

            //use one of the trigger methods (ie Wait() to make sure AggregateException
            //is observed)
            //t1.Wait();

            if (t1.IsFaulted)
            {

                Console.WriteLine("IsFaulted == true");
            }

            try
            {
                Console.WriteLine(t1.Result);
            }
            catch (AggregateException aggEx)
            {
                foreach (var ex in aggEx.InnerExceptions)
                {
                    Console.WriteLine("Caught exception '{0}'", ex.Message);
                }
            }
            finally
            {
                if (t1.IsFaulted)
                {
                    Console.WriteLine("IsFaulted == true");
                }
            }

            Console.WriteLine("End Method");
        }

        private static void SimpleTaskWithExceptionHandle()
        {
            Console.WriteLine("Start Method");

            // Create a task and start, use this 95% of the time. Use .Start() if you need to schedule a continuation within the task action.
            var t1 = Task.Factory.StartNew(() =>
                                           {
                                               throw new Exception("Exception Message !!!");

                                               return "Hello from Task";
                                           });


            try
            {
                //use one of the trigger methods (ie Wait() to make sure AggregateException
                //is observed)
                t1.Wait();

                if (t1.IsFaulted)
                {
                    //note this is not called.
                    Console.WriteLine("IsFaulted == true");
                }
            }
            catch (AggregateException aggEx)
            {
                foreach (var ex in aggEx.InnerExceptions)
                {
                    Console.WriteLine("Caught exception '{0}'", ex.Message);
                }
            }


            Console.WriteLine("End Method");
        }

        private static void SimpleTaskWithException()
        {
            Console.WriteLine("Start Method");

            // Create a task and start, use this 95% of the time. Use .Start() if you need to schedule a continuation within the task action.
            
            
            var t1 = Task.Factory.StartNew(() =>
                                           {
                                               throw new Exception("Exception Message !!!");

                                               return "Hello from Task";
                                           });

            //1.
            t1.Wait();

            //2.
            //Console.WriteLine(t1.Result);

            Console.WriteLine("End Method");
        }

        private static void SimpleTaskExmapleWithReturnValue()
        {
            Console.WriteLine("Start Method");

            // Create a task and start, use this 95% of the time. Use .Start() if you need to schedule a continuation within the task action.
            Task<string> t1 = Task.Factory.StartNew(() => { return "Hello from Task"; });

            t1.Wait();

            Console.WriteLine(t1.Result);
        }

        private static void SimpleTaskExmapleWithCorrectCreation()
        {
            Console.WriteLine("Start Method");


            // Create a task and start
            var t1 = Task.Factory.StartNew(() => { Console.WriteLine("Hello from Task"); });

            Console.WriteLine("End Method");

            t1.Wait();
        }

        private static void SimpleTaskExample()
        {
            Console.WriteLine("Start Method");

    
            // Create a task but do not start it.
            var t1 = new Task(Somefunction);

            t1.Start();

            Console.WriteLine("End Method");

            t1.Wait();
        }

        private static void Somefunction()
        {
            Console.WriteLine("Hello from Task");
        }
    }
}
