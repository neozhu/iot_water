开启监听数据修改功能
ALTER DATABASE [aspnet-Demo] SET NEW_BROKER
ALTER DATABASE [aspnet-Demo] SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE

GRANT SUBSCRIBE QUERY NOTIFICATIONS TO "DESKTOP-RH4DFBA\office"
ALTER AUTHORIZATION ON DATABASE::[aspnet-Demo] TO "DESKTOP-RH4DFBA\office";
select * from sys.transmission_queue
select * from sys.dm_qn_subscriptions