// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SpreadsheetUtilities
{
    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<String, HashSet<String>> dependents;
        private Dictionary<String, HashSet<String>> dependees;
        private int size;
        
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            this.dependents = new Dictionary<string, HashSet<string>>();
            this.dependees = new Dictionary<string, HashSet<string>>();
            this.size = 0;
        }

        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return this.size; }
        }

        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                try 
                { 
                    return this.dependees[s].Count; 
                }

                catch 
                { 
                    return 0; 
                }
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            try
            {
                return this.dependents.Count == 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            try
            {
                return this.dependees.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (this.HasDependents(s))
            {
                return this.dependents[s];
            }
            else
            {
                HashSet<string> empty = new HashSet<string>();
                return empty;
            }
        }
        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (this.HasDependees(s))
            {
                return this.dependees[s];
            }
            else
            {
                HashSet<string> empty = new HashSet<string>();
                return empty;
            }
        }

        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t(dent/child) depends on s(dee/parent)
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>         
        public void AddDependency(string s, string t)
        {
            // First, check if dependents[s] even exists.  
            if (this.dependents.ContainsKey(s))
            {
                // Great!  Now we need to check if it already has t
                if (this.dependents[s].Contains(t))
                {
                    // splendid!  No work required!
                    return;
                }
                else // we need to add t to dependents[s]
                {
                    this.dependents[s].Add(t);
                }
            }
            else // Then we need to create dependents[s]
            {
                HashSet<string> newDependent = new HashSet<string>();
                newDependent.Add(t);
                this.dependents.Add(s, newDependent);

                //we also have to create a dependee for t and s
                if (this.HasDependees(t))
                {
                    if (this.dependees.ContainsKey(t))
                    {
                        this.dependees[t].Add(s);
                    }
                    else
                    {
                        HashSet<string> newDependee = new HashSet<string>();
                        newDependee.Add(s);
                        this.dependees.Add(t, newDependee);
                    }
                }
            }
            this.size++;
            
        }

        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            // check if s even has dependnts
            if (this.HasDependents(s))
            {
                this.dependents[s].Remove(t);
                this.size--;

                // if s has no other dependents, then just get rid of s altogether
                if (!this.HasDependents(s))
                {
                    this.dependents.Remove(s);
                }

                if (this.HasDependees(s))
                {
                    this.dependees[t].Remove(s);

                    // if t has no other dependees, then just get rid of s altogether
                    if (!this.HasDependees(t))
                    {
                        this.dependees.Remove(s);
                    }
                }
            }
            // check that s has not dependees
            else if (this.HasDependees(s)) 
            {
                this.dependees[s].Remove(t);
                this.size--;

                // if s has no other dependees, then just get rid of s altogether
                if (!this.HasDependees(s))
                {
                    this.dependents.Remove(s);
                }

                if (this.HasDependents(t))
                {
                    this.dependents[t].Remove(s);

                    // if t has no other dependents, then just get rid of s altogether
                    if (this.HasDependents(t) == false)
                        this.dependents.Remove(s);
                }
            }
            else // if there are no dependents or dependeees
            {
                // No action required!
                return;
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            HashSet<string> replacement = new HashSet<string>(GetDependents(s));
            // Before we can replace the dependents, we need to remove all of s's dependees
            foreach (string dependee in replacement)
            {
                RemoveDependency(s, dependee);
            }
            // now we can add the new dependents to s
            foreach (string dependent in newDependents)
            {
                AddDependency(s, dependent);
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            HashSet<string> replacement = new HashSet<string>(GetDependees(s));
            // Before we can replace the dependees, we need to remove all s's dependees
            foreach (string dependee in replacement)
            {
                RemoveDependency(dependee, s);
            }
            // now we can add the new dependees to s
            foreach (string dependee in newDependees)
            {
                AddDependency(dependee, s);
            }
        }
    }
}
