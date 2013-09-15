using CEngineSharp_Server.Utilities;
using System;
using System.IO;

namespace CEngineSharp_Server.World
{
    public class Player : Entity
    {
        private string _password;
        private bool _loggedIn = false;

        public bool LoggedIn
        {
            get { return _loggedIn; }

            set { _loggedIn = value; }
        }

        public override void Kill(Entity killer)
        {
        }

        public void Load(string playerName)
        {
            try
            {
                string filePath = Constants.FilePath_Accounts + playerName + ".dat";

                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        this.Name = br.ReadString();
                        this.Level = br.ReadUInt16();
                        _password = br.ReadString();

                        foreach (Vitals vital in Enum.GetValues(typeof(Vitals)))
                        {
                            SetVital(vital, br.ReadUInt16());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Let the error handler take care of this problem; since it's an error only effecting a particular player, flag it as a low level error.
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public void Save()
        {
            try
            {
                string filePath = Constants.FilePath_Accounts + this.Name + ".dat";

                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(this.Name);
                        bw.Write(this.Level);
                        bw.Write(_password);

                        foreach (Vitals vital in Enum.GetValues(typeof(Vitals)))
                        {
                            bw.Write(GetVital(vital));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Let the error handler take care of this problem; since it's an error only effecting a particular player, flag it as a low level error.
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public static bool CheckName(string name)
        {
            try
            {
                string[] names = File.ReadAllLines((@"C:\Users\John\Desktop\names.txt"));

                foreach (var playernames in names)
                {
                    if (playernames == name)
                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Let the error handler take care of this problem; since it's an error affecting any registration or login attempts, flag it as a high level error.
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.High);

                // Just here for compiler-satisfaction - we'll never actually return anything as the server will terminate due to the exception, anyway,
                return false;
            }
        }

        public static void AddName(string name)
        {
            try
            {
                using (StreamWriter streamWriter = File.AppendText(Constants.FilePath_Data + "names.txt"))
                {
                    streamWriter.Write(name);
                }
            }
            catch (Exception ex)
            {
                // Let the error handler take care of this problem; since it's an error affecting any registration or login attempts, flag it as a high level error.
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.High);
            }
        }

        private bool CheckPassword(string name, string password)
        {
            Load(name);

            return (_password == password);
        }

        public bool Authenticate(string name, string password)
        {
            if (!CheckName(name)) return false;

            if (!CheckPassword(name, password)) return false;

            return true;
        }

        public bool Register(string name, string password)
        {
            if (CheckName(name)) return false;

            this.Name = name;
            _password = password;
            this.Level = 1;

            foreach (Vitals vital in Enum.GetValues(typeof(Vitals)))
            {
                SetVital(vital, 10);
            }

            Save();

            AddName(name);

            return true;
        }
    }
}