using System;
using System.Collections;
using System.Security.Principal;

using Cuyahoga.Core.DAL;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Summary description for User.
	/// </summary>
	[Serializable]
	public class User : IIdentity
	{
		private int _id;
		private string _userName;
		private string _password;
		private string _firstName;
		private string _lastName;
		private string _email;
		private DateTime _lastLogin;
		private string _lastIp;
		private bool _isAuthenticated;
		private IList _roles;
		private AccessLevel[] _permissions;
		private DateTime _updateTimestamp;

		#region properties
		/// <summary>
		/// Property Id (int)
		/// </summary>
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

        /// <summary>
        /// Property UserName (string)
        /// </summary>
        public string UserName
        {
        	get { return this._userName; }
        	set { this._userName = value; }
        }

		/// <summary>
		/// Property Password (string). Internally the MD5 hash of the password is used.
		/// </summary>
		public string Password
		{
			get { return this._password; }
			set 
			{
				SetPassword(value);
			}
		}

		/// <summary>
		/// Property FirstName (string)
		/// </summary>
		public string FirstName
		{
			get { return this._firstName; }
			set { this._firstName = value; }
		}

		/// <summary>
		/// Property LastName (string)
		/// </summary>
		public string LastName
		{
			get { return this._lastName; }
			set { this._lastName = value; }
		}

		/// <summary>
		/// Property Email (string)
		/// </summary>
		public string Email
		{
			get { return this._email; }
			set { this._email = value; }
		}

		/// <summary>
		/// Property LastLogin (DateTime)
		/// </summary>
		public DateTime LastLogin
		{
			get { return this._lastLogin; }
			set { this._lastLogin = value; }
		}

		/// <summary>
		/// Property LastIp (string)
		/// </summary>
		public string LastIp
		{
			get { return this._lastIp; }
			set { this._lastIp = value; }
		}

		/// <summary>
		/// Property Roles (IList)
		/// </summary>
		public IList Roles
		{
			get { return this._roles; }
			set { this._roles = value; }
		}

		/// <summary>
		/// IIdentity property <see cref="System.Security.Principal.IIdentity" />.
		/// </summary>
		public bool IsAuthenticated
		{
			get { return this._isAuthenticated; }
		}

		/// <summary>
		/// IIdentity property. 
		/// <remark>Returns a string with the Id of the user and not the username</remark>
		/// </summary>
		public string Name
		{
			get
			{ 
				if (this._isAuthenticated)
					return this._id.ToString();
				else
					return "";
			}
		}

		/// <summary>
		/// IIdentity property <see cref="System.Security.Principal.IIdentity" />.
		/// </summary>
		public string AuthenticationType
		{
			get	{ return "CuyahogaAuthentication"; }
		}

		/// <summary>
		/// Property UpdateTimestamp (DateTime)
		/// </summary>
		public DateTime UpdateTimestamp
		{
			get { return this._updateTimestamp; }
			set { this._updateTimestamp = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public AccessLevel[] Permissions
		{
			get 
			{
				if (this._permissions.Length == 0)
				{
					ArrayList permissions = new ArrayList();
					foreach (Role role in this.Roles)
					{
						foreach (AccessLevel permission in role.Permissions)
						{
							if (permissions.IndexOf(permission) == -1)
								permissions.Add(permission);
						}
					}
					this._permissions = (AccessLevel[])permissions.ToArray(typeof(AccessLevel));
				}
				return this._permissions;
			}
		}

		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public User()
		{
			this._id = -1;
			this._isAuthenticated = false;
			this._permissions = new AccessLevel[0];
		}

		/// <summary>
		/// Try to log-in the user with the username and password. 
		/// </summary>
		/// <returns></returns>
		public bool Login()
		{
            ICmsDataProvider dp = CmsDataFactory.GetInstance();
            dp.GetUserByUsernameAndPassword(this.UserName, this.Password, this);
			this._isAuthenticated = (this._id > 0);
			return this._isAuthenticated;
		}

		/// <summary>
		/// Check if the user has the requested access rights.
		/// </summary>
		/// <param name="accessLevel"></param>
		/// <returns></returns>
		public bool HasPermission(AccessLevel permission)
		{
			return Array.IndexOf(this.Permissions, permission) > -1;
		}

		/// <summary>
		/// Indicates if the user has view permissions for a certain Node.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool CanView(Node node)
		{
			foreach (Permission p in node.NodePermissions)
			{
				if (p.ViewAllowed && this.Roles.Contains(p.Role))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Indicates if the user has view permissions for a certain Section.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public bool CanView(Section section)
		{
			foreach (Permission p in section.SectionPermissions)
			{
				if (p.ViewAllowed && this.Roles.Contains(p.Role))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Indicates if the user has edit permissions for a certain Section.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public bool CanEdit(Section section)
		{
			foreach (Permission p in section.SectionPermissions)
			{
				if (this.Roles.Contains(p.Role))
				{
					return true;
				}
			}
			return false;
		}

		private void SetPassword(string password)
		{
			// Very simple password rule. Extend here when required.
			if (password.Length >= 5)
			{
				this._password = Util.Encryption.StringToMD5Hash(password);
				System.Diagnostics.Trace.WriteLine("MD5 result = " + this._password);
			}
			else
			{
				throw new ArgumentException("The password must contain at least 5 characters");
			}
		}
	}
}
