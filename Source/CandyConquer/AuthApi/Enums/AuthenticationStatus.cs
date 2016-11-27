// Project by Bauss
using System;

namespace CandyConquer.AuthApi.Enums
{
	/// <summary>
	/// Enumeration of authentication statuses.
	/// </summary>
	public enum AuthenticationStatus : uint
	{
		/// <summary>
		/// The account is banned.
		/// </summary>
		Banned = 0,
		/// <summary>
		/// The account id or password is invalid.
		/// </summary>
        InvalidAccountIDOrPassword = 1,
        /// <summary>
        /// The account is ready for login.
        /// </summary>
        Ready = 2,
        /// <summary>
        /// The point card has expired.
        /// </summary>
        PointCardExpired = 6,
        /// <summary>
        /// The monthly card has expired.
        /// </summary>
        MonthlyCardExpired = 7,
        /// <summary>
        /// The server is down.
        /// </summary>
        ServerIsDown = 10,
        /// <summary>
        /// Alerts please try again later.
        /// </summary>
        PleaseTryAgainLater = 11,
        /// <summary>
        /// The account is banned.
        /// </summary>
        AccountBanned = 12,
        /// <summary>
        /// The server is busy.
        /// </summary>
        ServerIsBusy = 20,
        /// <summary>
        /// The server is busy, try again later.
        /// </summary>
        ServerIsBusyTryAgainLater = 21,
        /// <summary>
        /// The account is locked.
        /// </summary>
        AccountLocked = 22,
        /// <summary>
        /// The account is not activated.
        /// </summary>
        AccountNotActivated = 30,
        /// <summary>
        /// Failed to activate account.
        /// </summary>
        FailedToActivateAccount = 31,
        /// <summary>
        /// Invalid input.
        /// </summary>
        InvalidInput = 40,
        /// <summary>
        /// Invalid info.
        /// </summary>
        InvalidInfo = 41,
        /// <summary>
        /// Timed out.
        /// </summary>
        TimedOut = 42,
        /// <summary>
        /// Recheck serial number.
        /// </summary>
        RecheckSerialNumber = 43,
        /// <summary>
        /// Unbound.
        /// </summary>
        Unbound = 46,
        /// <summary>
        /// Used 3 login attempts.
        /// </summary>
        Used3LoginAttempts = 51,
        /// <summary>
        /// Failed to login.
        /// </summary>
        FailedToLogin = 52,
        /// <summary>
        /// Database error.
        /// </summary>
        DatebaseError = 54,
        /// <summary>
        /// Invalid account id.
        /// </summary>
        InvalidAccountID = 57,
        /// <summary>
        /// The server is not configured.
        /// </summary>
        ServersNotConfigured = 59,
        /// <summary>
        /// The server is locked.
        /// </summary>
        ServerLocked = 70,
        /// <summary>
        /// The account has been locked by phone.
        /// </summary>
        AccountLockedbyPhone = 72
	}
}
