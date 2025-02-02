﻿using FaceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetACSEvent
{
    class GetAcsEventType
    {
        public static uint ReturnMinorTypeValue(ref string IndexValue)
        {
            if (MinorTypeDictionary.ContainsKey(IndexValue))
            {
                return MinorTypeDictionary[IndexValue];
            }
            return 0;
        }

        public static uint ReturnMajorTypeValue(ref string IndexValue)
        {
            if(MajorTypeDictionary.ContainsKey(IndexValue))
            {
                return MajorTypeDictionary[IndexValue];
            }
            return 0;
        }

        public static ushort ReturnInductiveEventTypeValue(string IndexValue)
        {
            if(ComInductiveEvent.ContainsKey(IndexValue))
            {
                return ComInductiveEvent[IndexValue];
            }
            return 0xffff;
        }

        public static int ReturnSearchValue(string IndexValue)
        {
            if(SearchType.ContainsKey(IndexValue))
            {
                return SearchType[IndexValue];
            }
            return 0;
        }
        private static Dictionary<string, uint> MinorTypeDictionary = new Dictionary<string, uint>()
        {
            {"All",0},
            {"ALARMIN_SHORT_CIRCUIT",CHCNetSDK.MINOR_ALARMIN_SHORT_CIRCUIT},
            {"ALARMIN_BROKEN_CIRCUIT",CHCNetSDK.MINOR_ALARMIN_BROKEN_CIRCUIT},
            {"ALARMIN_EXCEPTION",CHCNetSDK.MINOR_ALARMIN_EXCEPTION},
            {"ALARMIN_RESUME",CHCNetSDK.MINOR_ALARMIN_RESUME},
            {"HOST_DESMANTLE_ALARM",CHCNetSDK.MINOR_HOST_DESMANTLE_ALARM},
            {"HOST_DESMANTLE_RESUME",CHCNetSDK.MINOR_HOST_DESMANTLE_RESUME},
            {"CARD_READER_DESMANTLE_ALARM",CHCNetSDK.MINOR_CARD_READER_DESMANTLE_ALARM},
            {"CARD_READER_DESMANTLE_RESUME",CHCNetSDK.MINOR_CARD_READER_DESMANTLE_RESUME},
            {"CASE_SENSOR_ALARM",CHCNetSDK.MINOR_CASE_SENSOR_ALARM},
            {"CASE_SENSOR_RESUME",CHCNetSDK.MINOR_CASE_SENSOR_RESUME},
            {"STRESS_ALARM",CHCNetSDK.MINOR_STRESS_ALARM},
            {"OFFLINE_ECENT_NEARLY_FULL",CHCNetSDK.MINOR_OFFLINE_ECENT_NEARLY_FULL},
            {"CARD_MAX_AUTHENTICATE_FAIL",CHCNetSDK.MINOR_CARD_MAX_AUTHENTICATE_FAIL},
            {"SD_CARD_FULL",CHCNetSDK.MINOR_SD_CARD_FULL},
            {"LINKAGE_CAPTURE_PIC",CHCNetSDK.MINOR_LINKAGE_CAPTURE_PIC},
            {"SECURITY_MODULE_DESMANTLE_ALARM",CHCNetSDK.MINOR_SECURITY_MODULE_DESMANTLE_ALARM},
            {"SECURITY_MODULE_DESMANTLE_RESUME",CHCNetSDK.MINOR_SECURITY_MODULE_DESMANTLE_RESUME},
            {"POS_START_ALARM",CHCNetSDK.MINOR_POS_START_ALARM},
            {"POS_END_ALARM",CHCNetSDK.MINOR_POS_END_ALARM},
            {"FACE_IMAGE_QUALITY_LOW",CHCNetSDK.MINOR_FACE_IMAGE_QUALITY_LOW},
            {"FINGE_RPRINT_QUALITY_LOW",CHCNetSDK.MINOR_FINGE_RPRINT_QUALITY_LOW},
            {"FIRE_IMPORT_BROKEN_CIRCUIT",CHCNetSDK.MINOR_FIRE_IMPORT_BROKEN_CIRCUIT},
            {"FIRE_IMPORT_RESUME",CHCNetSDK.MINOR_FIRE_IMPORT_RESUME},
            {"FIRE_BUTTON_TRIGGER",CHCNetSDK.MINOR_FIRE_BUTTON_TRIGGER},
            {"FIRE_BUTTON_RESUME",CHCNetSDK.MINOR_FIRE_BUTTON_RESUME},
            {"MAINTENANCE_BUTTON_TRIGGER",CHCNetSDK.MINOR_MAINTENANCE_BUTTON_TRIGGER},
            {"MAINTENANCE_BUTTON_RESUME",CHCNetSDK.MINOR_MAINTENANCE_BUTTON_RESUME},
            {"EMERGENCY_BUTTON_TRIGGER",CHCNetSDK.MINOR_EMERGENCY_BUTTON_TRIGGER},
            {"EMERGENCY_BUTTON_RESUME",CHCNetSDK.MINOR_EMERGENCY_BUTTON_RESUME},
            {"DISTRACT_CONTROLLER_ALARM",CHCNetSDK.MINOR_DISTRACT_CONTROLLER_ALARM},
            {"DISTRACT_CONTROLLER_RESUME",CHCNetSDK.MINOR_DISTRACT_CONTROLLER_RESUME},
            {"CHANNEL_CONTROLLER_DESMANTLE_ALARM",CHCNetSDK.MINOR_CHANNEL_CONTROLLER_DESMANTLE_ALARM},
            {"CHANNEL_CONTROLLER_DESMANTLE_RESUME",CHCNetSDK.MINOR_CHANNEL_CONTROLLER_DESMANTLE_RESUME},
            {"CHANNEL_CONTROLLER_FIRE_IMPORT_ALARM",CHCNetSDK.MINOR_CHANNEL_CONTROLLER_FIRE_IMPORT_ALARM},
            {"CHANNEL_CONTROLLER_FIRE_IMPORT_RESUME",CHCNetSDK.MINOR_CHANNEL_CONTROLLER_FIRE_IMPORT_RESUME},
            {"PRINTER_OUT_OF_PAPER",CHCNetSDK.MINOR_PRINTER_OUT_OF_PAPER},
            {"LEGAL_EVENT_NEARLY_FULL",CHCNetSDK.MINOR_LEGAL_EVENT_NEARLY_FULL},
            {"NET_BROKEN",CHCNetSDK.MINOR_NET_BROKEN},
            {"RS485_DEVICE_ABNORMAL",CHCNetSDK.MINOR_RS485_DEVICE_ABNORMAL},
            {"RS485_DEVICE_REVERT",CHCNetSDK.MINOR_RS485_DEVICE_REVERT},
            {"DEV_POWER_ON",CHCNetSDK.MINOR_DEV_POWER_ON},
            {"DEV_POWER_OFF",CHCNetSDK.MINOR_DEV_POWER_OFF},
            {"WATCH_DOG_RESET",CHCNetSDK.MINOR_WATCH_DOG_RESET},
            {"LOW_BATTERY",CHCNetSDK.MINOR_LOW_BATTERY},
            {"BATTERY_RESUME",CHCNetSDK.MINOR_BATTERY_RESUME},
            {"AC_OFF",CHCNetSDK.MINOR_AC_OFF},
            {"AC_RESUME",CHCNetSDK.MINOR_AC_RESUME},
            {"NET_RESUME",CHCNetSDK.MINOR_NET_RESUME},
            {"FLASH_ABNORMAL",CHCNetSDK.MINOR_FLASH_ABNORMAL},
            {"CARD_READER_OFFLINE",CHCNetSDK.MINOR_CARD_READER_OFFLINE},
            {"CARD_READER_RESUME",CHCNetSDK.MINOR_CARD_READER_RESUME},
            {"INDICATOR_LIGHT_OFF",CHCNetSDK.MINOR_INDICATOR_LIGHT_OFF},
            {"INDICATOR_LIGHT_RESUME",CHCNetSDK.MINOR_INDICATOR_LIGHT_RESUME},
            {"CHANNEL_CONTROLLER_OFF",CHCNetSDK.MINOR_CHANNEL_CONTROLLER_OFF},
            {"CHANNEL_CONTROLLER_RESUME",CHCNetSDK.MINOR_CHANNEL_CONTROLLER_RESUME},
            {"SECURITY_MODULE_OFF",CHCNetSDK.MINOR_SECURITY_MODULE_OFF},
            {"SECURITY_MODULE_RESUME",CHCNetSDK.MINOR_SECURITY_MODULE_RESUME},
            {"BATTERY_ELECTRIC_LOW",CHCNetSDK.MINOR_BATTERY_ELECTRIC_LOW},
            {"BATTERY_ELECTRIC_RESUME",CHCNetSDK.MINOR_BATTERY_ELECTRIC_RESUME},
            {"LOCAL_CONTROL_NET_BROKEN",CHCNetSDK.MINOR_LOCAL_CONTROL_NET_BROKEN},
            {"LOCAL_CONTROL_NET_RSUME",CHCNetSDK.MINOR_LOCAL_CONTROL_NET_RSUME},
            {"MASTER_RS485_LOOPNODE_BROKEN",CHCNetSDK.MINOR_MASTER_RS485_LOOPNODE_BROKEN},
            {"MASTER_RS485_LOOPNODE_RESUME",CHCNetSDK.MINOR_MASTER_RS485_LOOPNODE_RESUME},
            {"LOCAL_CONTROL_OFFLINE",CHCNetSDK.MINOR_LOCAL_CONTROL_OFFLINE},
            {"LOCAL_CONTROL_RESUME",CHCNetSDK.MINOR_LOCAL_CONTROL_RESUME},
            {"LOCAL_DOWNSIDE_RS485_LOOPNODE_BROKEN",CHCNetSDK.MINOR_LOCAL_DOWNSIDE_RS485_LOOPNODE_BROKEN},
            {"LOCAL_DOWNSIDE_RS485_LOOPNODE_RESUME",CHCNetSDK.MINOR_LOCAL_DOWNSIDE_RS485_LOOPNODE_RESUME},
            {"DISTRACT_CONTROLLER_ONLINE",CHCNetSDK.MINOR_DISTRACT_CONTROLLER_ONLINE},
            {"DISTRACT_CONTROLLER_OFFLINE",CHCNetSDK.MINOR_DISTRACT_CONTROLLER_OFFLINE},
            {"ID_CARD_READER_NOT_CONNECT",CHCNetSDK.MINOR_ID_CARD_READER_NOT_CONNECT},
            {"ID_CARD_READER_RESUME",CHCNetSDK.MINOR_ID_CARD_READER_RESUME},
            {"FINGER_PRINT_MODULE_NOT_CONNECT",CHCNetSDK.MINOR_FINGER_PRINT_MODULE_NOT_CONNECT},
            {"FINGER_PRINT_MODULE_RESUME",CHCNetSDK.MINOR_FINGER_PRINT_MODULE_RESUME},
            {"CAMERA_NOT_CONNECT",CHCNetSDK.MINOR_CAMERA_NOT_CONNECT},
            {"CAMERA_RESUME",CHCNetSDK.MINOR_CAMERA_RESUME},
            {"COM_NOT_CONNECT",CHCNetSDK.MINOR_COM_NOT_CONNECT},
            {"COM_RESUME",CHCNetSDK.MINOR_COM_RESUME},
            {"DEVICE_NOT_AUTHORIZE",CHCNetSDK.MINOR_DEVICE_NOT_AUTHORIZE},
            {"PEOPLE_AND_ID_CARD_DEVICE_ONLINE",CHCNetSDK.MINOR_PEOPLE_AND_ID_CARD_DEVICE_ONLINE},
            {"PEOPLE_AND_ID_CARD_DEVICE_OFFLINE",CHCNetSDK.MINOR_PEOPLE_AND_ID_CARD_DEVICE_OFFLINE},
            {"LOCAL_LOGIN_LOCK",CHCNetSDK.MINOR_LOCAL_LOGIN_LOCK},
            {"LOCAL_LOGIN_UNLOCK",CHCNetSDK.MINOR_LOCAL_LOGIN_UNLOCK},
            {"SUBMARINEBACK_COMM_BREAK",CHCNetSDK.MINOR_SUBMARINEBACK_COMM_BREAK},
            {"SUBMARINEBACK_COMM_RESUME",CHCNetSDK.MINOR_SUBMARINEBACK_COMM_RESUME},
            {"MOTOR_SENSOR_EXCEPTION",CHCNetSDK.MINOR_MOTOR_SENSOR_EXCEPTION},
            {"CAN_BUS_EXCEPTION",CHCNetSDK.MINOR_CAN_BUS_EXCEPTION},
            {"CAN_BUS_RESUME",CHCNetSDK.MINOR_CAN_BUS_RESUME},
            {"GATE_TEMPERATURE_OVERRUN",CHCNetSDK.MINOR_GATE_TEMPERATURE_OVERRUN},
            {"IR_EMITTER_EXCEPTION",CHCNetSDK.MINOR_IR_EMITTER_EXCEPTION},
            {"IR_EMITTER_RESUME",CHCNetSDK.MINOR_IR_EMITTER_RESUME},
            {"LAMP_BOARD_COMM_EXCEPTION",CHCNetSDK.MINOR_LAMP_BOARD_COMM_EXCEPTION},
            {"LAMP_BOARD_COMM_RESUME",CHCNetSDK.MINOR_LAMP_BOARD_COMM_RESUME},
            {"IR_ADAPTOR_COMM_EXCEPTION",CHCNetSDK.MINOR_IR_ADAPTOR_COMM_EXCEPTION},
            {"IR_ADAPTOR_COMM_RESUME",CHCNetSDK.MINOR_IR_ADAPTOR_COMM_RESUME},
            {"PRINTER_ONLINE",CHCNetSDK.MINOR_PRINTER_ONLINE},
            {"PRINTER_OFFLINE",CHCNetSDK.MINOR_PRINTER_OFFLINE},
            {"4G_MOUDLE_ONLINE",CHCNetSDK.MINOR_4G_MOUDLE_ONLINE},
            {"4G_MOUDLE_OFFLINE",CHCNetSDK.MINOR_4G_MOUDLE_OFFLINE},
            {"LOCAL_UPGRADE",CHCNetSDK.MINOR_LOCAL_UPGRADE},
            {"REMOTE_LOGIN",CHCNetSDK.MINOR_REMOTE_LOGIN},
            {"REMOTE_LOGOUT",CHCNetSDK.MINOR_REMOTE_LOGOUT},
            {"MINOR_REMOTE_ARM",CHCNetSDK.MINOR_REMOTE_ARM},
            {"REMOTE_DISARM",CHCNetSDK.MINOR_REMOTE_DISARM},
            {"REMOTE_REBOOT",CHCNetSDK.MINOR_REMOTE_REBOOT},
            {"REMOTE_UPGRADE",CHCNetSDK.MINOR_REMOTE_UPGRADE},
            {"REMOTE_CFGFILE_OUTPUT",CHCNetSDK.MINOR_REMOTE_CFGFILE_OUTPUT},
            {"REMOTE_CFGFILE_INTPUT",CHCNetSDK.MINOR_REMOTE_CFGFILE_INTPUT},
            {"REMOTE_ALARMOUT_OPEN_MAN",CHCNetSDK.MINOR_REMOTE_ALARMOUT_OPEN_MAN},
            {"REMOTE_ALARMOUT_CLOSE_MAN",CHCNetSDK.MINOR_REMOTE_ALARMOUT_CLOSE_MAN},
            {"REMOTE_OPEN_DOOR",CHCNetSDK.MINOR_REMOTE_OPEN_DOOR},
            {"REMOTE_CLOSE_DOOR",CHCNetSDK.MINOR_REMOTE_CLOSE_DOOR},
            {"REMOTE_ALWAYS_OPEN",CHCNetSDK.MINOR_REMOTE_ALWAYS_OPEN},
            {"REMOTE_ALWAYS_CLOSE",CHCNetSDK.MINOR_REMOTE_ALWAYS_CLOSE},
            {"REMOTE_CHECK_TIME",CHCNetSDK.MINOR_REMOTE_CHECK_TIME},
            {"NTP_CHECK_TIME",CHCNetSDK.MINOR_NTP_CHECK_TIME},
            {"REMOTE_CLEAR_CARD",CHCNetSDK.MINOR_REMOTE_CLEAR_CARD},
            {"REMOTE_RESTORE_CFG",CHCNetSDK.MINOR_REMOTE_RESTORE_CFG},
            {"ALARMIN_ARM",CHCNetSDK.MINOR_ALARMIN_ARM},
            {"ALARMIN_DISARM",CHCNetSDK.MINOR_ALARMIN_DISARM},
            {"LOCAL_RESTORE_CFG",CHCNetSDK.MINOR_LOCAL_RESTORE_CFG},
            {"REMOTE_CAPTURE_PIC",CHCNetSDK.MINOR_REMOTE_CAPTURE_PIC},
            {"MOD_NET_REPORT_CFG",CHCNetSDK.MINOR_MOD_NET_REPORT_CFG},
            {"MOD_GPRS_REPORT_PARAM",CHCNetSDK.MINOR_MOD_GPRS_REPORT_PARAM},
            {"MOD_REPORT_GROUP_PARAM",CHCNetSDK.MINOR_MOD_REPORT_GROUP_PARAM},
            {"UNLOCK_PASSWORD_OPEN_DOOR",CHCNetSDK.MINOR_UNLOCK_PASSWORD_OPEN_DOOR},
            {"AUTO_RENUMBER",CHCNetSDK.MINOR_AUTO_RENUMBER},
            {"AUTO_COMPLEMENT_NUMBER",CHCNetSDK.MINOR_AUTO_COMPLEMENT_NUMBER},
            {"NORMAL_CFGFILE_INPUT",CHCNetSDK.MINOR_NORMAL_CFGFILE_INPUT},
            {"NORMAL_CFGFILE_OUTTPUT",CHCNetSDK.MINOR_NORMAL_CFGFILE_OUTTPUT},
            {"CARD_RIGHT_INPUT",CHCNetSDK.MINOR_CARD_RIGHT_INPUT},
            {"CARD_RIGHT_OUTTPUT",CHCNetSDK.MINOR_CARD_RIGHT_OUTTPUT},
            {"LOCAL_USB_UPGRADE",CHCNetSDK.MINOR_LOCAL_USB_UPGRADE},
            {"REMOTE_VISITOR_CALL_LADDER",CHCNetSDK.MINOR_REMOTE_VISITOR_CALL_LADDER},
            {"REMOTE_HOUSEHOLD_CALL_LADDER",CHCNetSDK.MINOR_REMOTE_HOUSEHOLD_CALL_LADDER},
            {"REMOTE_ACTUAL_GUARD",CHCNetSDK.MINOR_REMOTE_ACTUAL_GUARD},
            {"REMOTE_ACTUAL_UNGUARD",CHCNetSDK.MINOR_REMOTE_ACTUAL_UNGUARD},
            {"REMOTE_CONTROL_NOT_CODE_OPER_FAILED",CHCNetSDK.MINOR_REMOTE_CONTROL_NOT_CODE_OPER_FAILED},
            {"REMOTE_CONTROL_CLOSE_DOOR",CHCNetSDK.MINOR_REMOTE_CONTROL_CLOSE_DOOR},
            {"REMOTE_CONTROL_OPEN_DOOR",CHCNetSDK.MINOR_REMOTE_CONTROL_OPEN_DOOR},
            {"REMOTE_CONTROL_ALWAYS_OPEN_DOOR",CHCNetSDK.MINOR_REMOTE_CONTROL_ALWAYS_OPEN_DOOR},
            {"LEGAL_CARD_PASS",CHCNetSDK.MINOR_LEGAL_CARD_PASS},
            {"CARD_AND_PSW_PASS",CHCNetSDK.MINOR_CARD_AND_PSW_PASS},
            {"CARD_AND_PSW_FAIL",CHCNetSDK.MINOR_CARD_AND_PSW_FAIL},
            {"CARD_AND_PSW_TIMEOUT",CHCNetSDK.MINOR_CARD_AND_PSW_TIMEOUT},
            {"CARD_AND_PSW_OVER_TIME",CHCNetSDK.MINOR_CARD_AND_PSW_OVER_TIME},
            {"CARD_NO_RIGHT",CHCNetSDK.MINOR_CARD_NO_RIGHT},
            {"CARD_INVALID_PERIOD",CHCNetSDK.MINOR_CARD_INVALID_PERIOD},
            {"CARD_OUT_OF_DATE",CHCNetSDK.MINOR_CARD_OUT_OF_DATE},
            {"INVALID_CARD",CHCNetSDK.MINOR_INVALID_CARD},
            {"ANTI_SNEAK_FAIL",CHCNetSDK.MINOR_ANTI_SNEAK_FAIL},
            {"INTERLOCK_DOOR_NOT_CLOSE",CHCNetSDK.MINOR_INTERLOCK_DOOR_NOT_CLOSE},
            {"NOT_BELONG_MULTI_GROUP",CHCNetSDK.MINOR_NOT_BELONG_MULTI_GROUP},
            {"INVALID_MULTI_VERIFY_PERIOD",CHCNetSDK.MINOR_INVALID_MULTI_VERIFY_PERIOD},
            {"MULTI_VERIFY_SUPER_RIGHT_FAIL",CHCNetSDK.MINOR_MULTI_VERIFY_SUPER_RIGHT_FAIL},
            {"MULTI_VERIFY_REMOTE_RIGHT_FAIL",CHCNetSDK.MINOR_MULTI_VERIFY_REMOTE_RIGHT_FAIL},
            {"MULTI_VERIFY_SUCCESS",CHCNetSDK.MINOR_MULTI_VERIFY_SUCCESS},
            {"LEADER_CARD_OPEN_BEGIN",CHCNetSDK.MINOR_LEADER_CARD_OPEN_BEGIN},
            {"LEADER_CARD_OPEN_END",CHCNetSDK.MINOR_LEADER_CARD_OPEN_END},
            {"ALWAYS_OPEN_BEGIN",CHCNetSDK.MINOR_ALWAYS_OPEN_BEGIN},
            {"ALWAYS_OPEN_END",CHCNetSDK.MINOR_ALWAYS_OPEN_END},
            {"LOCK_OPEN",CHCNetSDK.MINOR_LOCK_OPEN},
            {"LOCK_CLOSE",CHCNetSDK.MINOR_LOCK_CLOSE},
            {"DOOR_BUTTON_PRESS",CHCNetSDK.MINOR_DOOR_BUTTON_PRESS},
            {"DOOR_BUTTON_RELEASE",CHCNetSDK.MINOR_DOOR_BUTTON_RELEASE},
            {"DOOR_OPEN_NORMAL",CHCNetSDK.MINOR_DOOR_OPEN_NORMAL},
            {"DOOR_CLOSE_NORMAL",CHCNetSDK.MINOR_DOOR_CLOSE_NORMAL},
            {"DOOR_OPEN_ABNORMAL",CHCNetSDK.MINOR_DOOR_OPEN_ABNORMAL},
            {"DOOR_OPEN_TIMEOUT",CHCNetSDK.MINOR_DOOR_OPEN_TIMEOUT},
            {"ALARMOUT_ON",CHCNetSDK.MINOR_ALARMOUT_ON},
            {"ALARMOUT_OFF",CHCNetSDK.MINOR_ALARMOUT_OFF},
            {"ALWAYS_CLOSE_BEGIN",CHCNetSDK.MINOR_ALWAYS_CLOSE_BEGIN},
            {"ALWAYS_CLOSE_END",CHCNetSDK.MINOR_ALWAYS_CLOSE_END},
            {"MULTI_VERIFY_NEED_REMOTE_OPEN",CHCNetSDK.MINOR_MULTI_VERIFY_NEED_REMOTE_OPEN},
            {"MULTI_VERIFY_SUPERPASSWD_VERIFY_SUCCESS",CHCNetSDK.MINOR_MULTI_VERIFY_SUPERPASSWD_VERIFY_SUCCESS},
            {"MULTI_VERIFY_REPEAT_VERIFY",CHCNetSDK.MINOR_MULTI_VERIFY_REPEAT_VERIFY},
            {"MULTI_VERIFY_TIMEOUT",CHCNetSDK.MINOR_MULTI_VERIFY_TIMEOUT},
            {"DOORBELL_RINGING",CHCNetSDK.MINOR_DOORBELL_RINGING},
            {"FINGERPRINT_COMPARE_PASS",CHCNetSDK.MINOR_FINGERPRINT_COMPARE_PASS},
            {"FINGERPRINT_COMPARE_FAIL",CHCNetSDK.MINOR_FINGERPRINT_COMPARE_FAIL},
            {"CARD_FINGERPRINT_VERIFY_PASS",CHCNetSDK.MINOR_CARD_FINGERPRINT_VERIFY_PASS},
            {"CARD_FINGERPRINT_VERIFY_FAIL",CHCNetSDK.MINOR_CARD_FINGERPRINT_VERIFY_FAIL},
            {"CARD_FINGERPRINT_VERIFY_TIMEOUT",CHCNetSDK.MINOR_CARD_FINGERPRINT_VERIFY_TIMEOUT},
            {"CARD_FINGERPRINT_PASSWD_VERIFY_PASS",CHCNetSDK.MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_PASS},
            {"CARD_FINGERPRINT_PASSWD_VERIFY_FAIL",CHCNetSDK.MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_FAIL},
            {"CARD_FINGERPRINT_PASSWD_VERIFY_TIMEOUT",CHCNetSDK.MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_TIMEOUT},
            {"FINGERPRINT_PASSWD_VERIFY_PASS",CHCNetSDK.MINOR_FINGERPRINT_PASSWD_VERIFY_PASS},
            {"FINGERPRINT_PASSWD_VERIFY_FAIL",CHCNetSDK.MINOR_FINGERPRINT_PASSWD_VERIFY_FAIL},
            {"FINGERPRINT_PASSWD_VERIFY_TIMEOUT",CHCNetSDK.MINOR_FINGERPRINT_PASSWD_VERIFY_TIMEOUT},
            {"FINGERPRINT_INEXISTENCE",CHCNetSDK.MINOR_FINGERPRINT_INEXISTENCE},
            {"CARD_PLATFORM_VERIFY",CHCNetSDK.MINOR_CARD_PLATFORM_VERIFY},
            {"MAC_DETECT",CHCNetSDK.MINOR_MAC_DETECT},
            {"LEGAL_MESSAGE",CHCNetSDK.MINOR_LEGAL_MESSAGE},
            {"ILLEGAL_MESSAGE",CHCNetSDK.MINOR_ILLEGAL_MESSAGE},
            {"DOOR_OPEN_OR_DORMANT_FAIL",CHCNetSDK.MINOR_DOOR_OPEN_OR_DORMANT_FAIL},
            {"AUTH_PLAN_DORMANT_FAIL",CHCNetSDK.MINOR_AUTH_PLAN_DORMANT_FAIL},
            {"CARD_ENCRYPT_VERIFY_FAIL",CHCNetSDK.MINOR_CARD_ENCRYPT_VERIFY_FAIL},
            {"SUBMARINEBACK_REPLY_FAIL",CHCNetSDK.MINOR_SUBMARINEBACK_REPLY_FAIL},
            {"DOOR_OPEN_OR_DORMANT_OPEN_FAIL",CHCNetSDK.MINOR_DOOR_OPEN_OR_DORMANT_OPEN_FAIL},
            {"DOOR_OPEN_OR_DORMANT_LINKAGE_OPEN_FAIL",CHCNetSDK.MINOR_DOOR_OPEN_OR_DORMANT_LINKAGE_OPEN_FAIL},
            {"TRAILING",CHCNetSDK.MINOR_TRAILING},
            {"REVERSE_ACCESS",CHCNetSDK.MINOR_REVERSE_ACCESS},
            {"FORCE_ACCESS",CHCNetSDK.MINOR_FORCE_ACCESS},
            {"CLIMBING_OVER_GATE",CHCNetSDK.MINOR_CLIMBING_OVER_GATE},
            {"PASSING_TIMEOUT",CHCNetSDK.MINOR_PASSING_TIMEOUT},
            {"INTRUSION_ALARM",CHCNetSDK.MINOR_INTRUSION_ALARM},
            {"FREE_GATE_PASS_NOT_AUTH",CHCNetSDK.MINOR_FREE_GATE_PASS_NOT_AUTH},
            {"DROP_ARM_BLOCK",CHCNetSDK.MINOR_DROP_ARM_BLOCK},
            {"DROP_ARM_BLOCK_RESUME",CHCNetSDK.MINOR_DROP_ARM_BLOCK_RESUME},
            {"LOCAL_FACE_MODELING_FAIL",CHCNetSDK.MINOR_LOCAL_FACE_MODELING_FAIL},
            {"STAY_EVENT",CHCNetSDK.MINOR_STAY_EVENT},
            {"PASSWORD_MISMATCH",CHCNetSDK.MINOR_PASSWORD_MISMATCH},
            {"EMPLOYEE_NO_NOT_EXIST",CHCNetSDK.MINOR_EMPLOYEE_NO_NOT_EXIST},
            {"COMBINED_VERIFY_PASS",CHCNetSDK.MINOR_COMBINED_VERIFY_PASS},
            {"COMBINED_VERIFY_TIMEOUT",CHCNetSDK.MINOR_COMBINED_VERIFY_TIMEOUT},
            {"VERIFY_MODE_MISMATCH",CHCNetSDK.MINOR_VERIFY_MODE_MISMATCH}
        };

        private static Dictionary<string, uint> MajorTypeDictionary = new Dictionary<string, uint>()
        {
            {"All",0},
            {"Alarm",1},
            {"Exception",2},
            {"Operation",3},
            {"Event",5}
        };

        private static Dictionary<string, ushort> ComInductiveEvent = new Dictionary<string, ushort>() 
        {
            {"invalid",0},
            {"authenticated",1},
            {"authenticationFailed",2},
            {"openingDoor",3},
            {"closingDoor",4},
            {"doorException",5},
            {"remoteOperation",6},
            {"timeSynchronization",7},
            {"deviceException",8},
            {"deviceRecovered",9},
            {"alarmTriggered",10},
            {"alarmRecovered",11},
            {"callCenter",12},
            {"all",0xffff}
        };

        public static int NumOfInductiveEvent()
        {
            return ComInductiveEvent.Count;
        }

        public static string FindKeyOfInductive(ushort value)
        {
            string res = "";
            foreach(var item in ComInductiveEvent)
            {
                if(item.Value==value)
                {
                    res = item.Key;
                }
            }
            return res;
        }
        private static Dictionary<string, int> SearchType = new Dictionary<string, int>() 
        {
            {"Invalid",0},
            {"Event source",1},
            {"Monitor ID",2}
        };
    }

}
