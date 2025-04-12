/***********************************

* Author    : lisonghappy
* Email     : lisonghappy@gmail.com
* Date      : 2025-04-11
* Desc      : IOCP Net Server session pool

************************************/


using System.Collections.Generic;

namespace IOCPNet {

    public class IOCPServerSessionPool<T_Session> where T_Session : IOCPSession, new() {

        private Stack<T_Session> sessionStack;
        public int Size => sessionStack.Count;



        public IOCPServerSessionPool (int capacity) {
            sessionStack = new Stack<T_Session>(capacity);
        }

        public T_Session Pop () {
            lock (sessionStack) {
                return sessionStack.Pop();
            }
        }

        public void Push (T_Session session) {
            if (session == null) {
                IOCPUtils.Logger.LogError("push session to pool cannot be null");
            }

            lock (sessionStack) {
                sessionStack.Push(session);
            }
        }
    }
}