using System;
using System.Collections.Generic;
using System.Messaging;

namespace MiscHelper
{
    public static class MSMQHelper
    {
        public static bool DeleteLocalQueue(string queName)
        {
            String daQue = $@".\Private$\{queName}";
            return DeleteQueue(daQue);
        }

        public static bool DeleteQueue(String que)
        {
            bool retVal = false;
            try
            {
                if (MessageQueue.Exists(que))
                {
                    MessageQueue.Delete(que);
                    retVal = true;
                }
            }
            catch { }
            return retVal;
        }

        public static MessageQueue GetLocalPrivateQueue(string queName)
        {
            String daQue = $@".\Private$\{queName}";
            MessageQueue mq = CreateGetQue(daQue);
            return mq;
        }

        public static MessageQueue CreateGetQue(string daQue)
        {
            MessageQueue mq;

            if (MessageQueue.Exists(daQue))
            {
                mq = new MessageQueue(daQue);
            }
            else
            {
                mq = MessageQueue.Create(daQue);
            }
            mq.Formatter = new BinaryMessageFormatter();
            return mq;
        }

        public static bool PostMessageToLocalQueue<T>(string queName, T objMessage)
        {
            bool ret = false;

            try
            {
                MessageQueue mq = GetLocalPrivateQueue(queName);
                PostMessage(objMessage, mq);
                ret = true;
            }
            catch { }

            return ret;
        }

        public static void PostMessageToQueue<T>(string queName, T objMessage)
        {
            MessageQueue mq = CreateGetQue(queName);
            PostMessage(objMessage, mq);
        }

        private static void PostMessage<T>(T objMessage, MessageQueue mq)
        {
            mq.Send(new Message(objMessage, mq.Formatter)); // , mq.Formatter
        }

        public static void ProcessMessagesFromQueue<T>(string queName, Func<T, bool> Processor)
        {
            TimeSpan waitTimeOut = TimeSpan.FromMilliseconds(200);

            MessageQueue mq = CreateGetQue(queName);

            ProcessMessagesFromQueue(mq, waitTimeOut, Processor);
        }

        public static void ProcessMessagesFromLocalQueue<T>(string queName, Func<T, bool> Processor)
        {
            TimeSpan waitTimeOut = TimeSpan.FromMilliseconds(200);

            MessageQueue mq = GetLocalPrivateQueue(queName);

            ProcessMessagesFromQueue(mq, waitTimeOut, Processor);
        }

        private static void ProcessMessagesFromQueue<T>(MessageQueue mq, TimeSpan waitTimeOut, Func<T, bool> Processor)
        {
            Message[] msgs = mq.GetAllMessages();
            List<String> processingList = new List<string>();

            foreach (Message msg in msgs)
            {
                processingList.Add(msg.Id);
            }

            foreach (var item in processingList)
            {
                try
                {
                    Message msg = mq.PeekById(item, waitTimeOut);

                    if (msg != null)
                    {
                        T data = (T)msg.Body;
                        if (true == Processor(data))
                        {
                            mq.ReceiveById(msg.Id, waitTimeOut);
                        }
                    }
                }
                catch { }
            }
        }

        public static bool TestLocalQueueCreation(out string errorMessage)
        {
            const string TESTVALUE = "What";
            errorMessage = "";

            Guid id = Guid.NewGuid();
            String key = id.ToString();
            bool retVal = false;

            try
            {
                MSMQHelper.PostMessageToLocalQueue<String>(key, TESTVALUE);

                String result = "";
                MSMQHelper.ProcessMessagesFromLocalQueue<String>(key, (input) =>
                {
                    result += input;
                    return true;
                });

                if (result != TESTVALUE)
                {
                    MSMQHelper.DeleteLocalQueue(key);
                    return false;
                }

                if (true == MSMQHelper.DeleteLocalQueue(key))
                    retVal = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }


            return retVal;
        }

    }
}
