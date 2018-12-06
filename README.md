# apache-config-builder
Remote apache http config builder 


The design seeks to enable a non-Linux savvy administrator to establish a basic Apache webserver on a Redhat 7/CentOS 7 Linux distribution remotely via a Windows host.  The interface provides both a Wizard style build approach, stepping through the configuration options, and a manual build approach, to install the HTTPD service, compile a httpd.conf basic configuration, and apply the configuration to the server.  The project is designed to interact with the remote Linux server via the SSH protocol connecting to a remote SSH server on port 22.
