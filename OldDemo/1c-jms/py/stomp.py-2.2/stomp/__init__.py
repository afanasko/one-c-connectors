"""
This provides basic connectivity to a message broker supporting the 'stomp' protocol.
At the moment ACK, SEND, SUBSCRIBE, UNSUBSCRIBE, BEGIN, ABORT, COMMIT, CONNECT and DISCONNECT operations
are supported.

See the project page for more information.

Meta-Data
---------
Author: Jason R Briggs
License: http://www.apache.org/licenses/LICENSE-2.0
Start Date: 2005/12/01
Last Revision Date: $Date: 2008/09/11 00:16 $
Project Page: http://www.briggs.net.nz/log/projects/stomp.py

Notes/Attribution
-----------------
 * uuid method courtesy of Carl Free Jr:
   http://aspn.activestate.com/ASPN/Cookbook/Python/Recipe/213761
 * patch from Andreas Schobel
 * patches from Julian Scheid of Rising Sun Pictures (http://open.rsp.com.au)
 * patch from Fernando
 * patches from Eugene Strulyov
"""

import stomp.internal
from stomp.internal import connect, listener, exception

#
# alias internals for module access
#
__version__ = stomp.internal.__version__
Connection = connect.Connection
ConnectionListener = listener.ConnectionListener
StatsListener = listener.StatsListener